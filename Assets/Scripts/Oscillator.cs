using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPosition;
    float movementFactor;

    [SerializeField] Vector3 movementVector; // values set within Unity
    [SerializeField] float period = 2f;
    
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        OscillateObstacle();
    }

    private void OscillateObstacle()
    {
        float cycles = Time.time / period; // continually growing over time
                                           // eg. 2 sec will complete 1 cycle; 2 sec / 2 (period) = 1, which indicates a lapse after 2 seconds

        const float tau = Mathf.PI * 2; // constant value of 6.283 - pi x 2 used for sine wav oscillation... math lol
        float rawSineWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        // Debug.Log(rawSineWave); // values in console are between -1 and 1

        movementFactor = (rawSineWave + 1f) / 2f; // adding 1f to rawSineWave so value goes from 0 to 1, to 0 to 2
                                                  // dividing by 2 recalculates to go back to 0 and 1 - cleaner code
                                                  // could also just divide rawSineWave by 2, but then oscillation will be between -0.5 and 0.5 - same effect
        
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPosition + offset;
    }
}
