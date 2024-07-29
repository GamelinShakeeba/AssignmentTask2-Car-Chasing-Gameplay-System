using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WCs;
    public GameObject[] Wheels;
    public float torque = 450;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 8000;

    public AudioSource skidSound;
    public AudioSource highAccel;

    public Transform skidTrialPrefab;
    Transform[] skidTrials = new Transform[4];
    public float skidThreshold = 0.4f;  // Threshold for detecting skid
    public float skidSoundCooldown = 0.5f; // Cooldown time in seconds
    private float lastSkidTime = 0f;

    public ParticleSystem smokePrefab;
    ParticleSystem[] skidSmoke = new ParticleSystem[4];

    public GameObject brakeLight;

    public Rigidbody rb;
    public float gearLength = 3;
    public float currentSpeed { get { return rb.velocity.magnitude * gearLength; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 500;

    public GameObject playerNamePrefab;

    public Renderer carMesh;

    string[] aiNames = { "Sam", "Raj", "Daya", "Geet", "Jetha" };

    public void StartSkidTrial(int i)
    {
        if (skidTrials[i] == null)
            skidTrials[i] = Instantiate(skidTrialPrefab);

        skidTrials[i].parent = WCs[i].transform;
        skidTrials[i].localRotation = Quaternion.Euler(90, 0, 0);
        skidTrials[i].localPosition = -Vector3.up * WCs[i].radius;
    }

    public void EndSkidTrial(int i)
    {
        if (skidTrials[i] == null) return;
        Transform holder = skidTrials[i];
        skidTrials[i] = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.gameObject, 30);
    }

    void Start()
    {
        SmokeInstantiate();
        brakeLight.SetActive(false);

        GameObject playerName = Instantiate(playerNamePrefab);
        playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;

        if (this.GetComponent<AIController>().enabled)
            playerName.GetComponent<Text>().text = aiNames[Random.Range(0, aiNames.Length)];
        else
            playerName.GetComponent<Text>().text = "PLAYER";
        
        playerName.GetComponent<NameUIController>().carRend = carMesh;
    }

    void SmokeInstantiate()
    {
        for (int i = 0; i < 4; i++)
        {
            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }
    }

    public void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
                                                    Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax)
            currentGear--;

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
            currentGear++;

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;
    }

    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        
        if(brake != 0)
            brakeLight.SetActive(true);
        else
            brakeLight.SetActive(false);

        float thrustTorque = 0;
        if(currentSpeed < maxSpeed)
            thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            WCs[i].motorTorque = thrustTorque;

            if(i < 2)
                WCs[i].steerAngle = steer;
            else
                WCs[i].brakeTorque = brake;

            Quaternion quat;
            Vector3 position;
            WCs[i].GetWorldPose(out position, out quat);
            Wheels[i].transform.position = position;
            Wheels[i].transform.rotation = quat;
        } 
    }

    public void CheckForSkid()
    {
        int numSkidding = 0;
        bool isSkidding = false;

        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            WCs[i].GetGroundHit(out wheelHit);

            // Adjusted thresholds for skid detection
            if (Mathf.Abs(wheelHit.forwardSlip) >= skidThreshold || Mathf.Abs(wheelHit.sidewaysSlip) >= skidThreshold)
            {
                numSkidding++;
                isSkidding = true;

                // Emit skid smoke
                skidSmoke[i].transform.position = WCs[i].transform.position - WCs[i].transform.up * WCs[i].radius;
                skidSmoke[i].Emit(1);
            }
        }
        // Manage skid sound
        if (isSkidding)
        {
            if (Time.time - lastSkidTime > skidSoundCooldown)
            {
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();
                }
                lastSkidTime = Time.time;
            }
        }
        else
        {
            if (skidSound.isPlaying)
            {
                skidSound.Stop();
            }
        }
    }
}
