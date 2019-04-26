using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private enum State { Alive, Transcending }

    private Rigidbody _rocket;
    private AudioSource _audio;
    private State _state;

    [SerializeField]
    private float MainThrust;

    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private float TranscnedingDelay;
    // Start is called before the first frame update
    void Start()
    {
        _rocket = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == State.Transcending)
        {
            return;
        }
        PlayAudio();
        ProcessInput();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state == State.Transcending)
        {
            return;
        }

        _state = State.Transcending;

        switch (collision.gameObject.tag)
        {
            case Constant.Tag.Untagged:
                Invoke("ResetGame", TranscnedingDelay);
                break;
            case Constant.Tag.Finish:
                Invoke("LoadNextScene", TranscnedingDelay);
                break;
            default:
                _state = State.Alive;
                break;
        }
    }

    private void LoadNextScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex == 1 ? 0 : 1);
    }

    private void ResetGame() => SceneManager.LoadScene(0);

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
