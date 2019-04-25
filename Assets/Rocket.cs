using Assets;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody _rocket;
    private AudioSource _audio;

    [SerializeField]
    private float MainThrust;

    [SerializeField]
    private float RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rocket = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayAudio();
        ProcessInput();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case Constant.Tag.Friendly:
                print("Friendly");
                break;
            default:
                print("Deadly");
                break;
        }
    }

    private void ProcessInput()
    {
        // Thrust
        if (Input.GetKey(KeyCode.Space))
        {
            _rocket.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);
        }

        // Rotate
        _rocket.freezeRotation = true; // To take manual control, and prevent exessive rotation when hitting something

        // Right
        var rotationForThisFrame = RotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
        {
            _rocket.transform.Rotate(Vector3.back * rotationForThisFrame);
        }
        
        // Left
        if (Input.GetKey(KeyCode.A))
        {
            _rocket.transform.Rotate(Vector3.forward * rotationForThisFrame);
        }
        _rocket.freezeRotation = false;
    }

    private void PlayAudio()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audio.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _audio.Pause();
        }
    }
}
