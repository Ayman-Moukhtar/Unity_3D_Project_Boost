using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private enum State { Alive, Transcending }

    private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    private Rigidbody _rocket;
    private AudioSource _audio;
    private State _state;
    private static bool _isDebugMode = false;

    [SerializeField] private float _mainThrustSpeed = 2500f;
    [SerializeField] private float _rotationSpeed = 250f;
    [SerializeField] private float _transcendingDelay = 1.5f;
    [SerializeField] private AudioClip _mainEngineAudio;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _levelLoadSound;
    [SerializeField] private ParticleSystem _mainEngineParticles;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private ParticleSystem _successParticles;

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
        ProcessInput();
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_state == State.Transcending)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case Constant.Tag.Untagged:
                if (_isDebugMode)
                {
                    break;
                }
                StartDeathSequence();
                break;
            case Constant.Tag.Finish:
                StartSuccessSequence();
                break;
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild)
        {
            LoadNextScene();
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && Debug.isDebugBuild)
        {
            _isDebugMode = !_isDebugMode;
        }
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audio.Stop();
        _audio.PlayOneShot(_levelLoadSound);
        _mainEngineParticles.Stop();
        _successParticles.Play();
        Invoke("LoadNextScene", _transcendingDelay);
    }

    private void StartDeathSequence()
    {
        _state = State.Transcending;
        _mainEngineParticles.Stop();
        _deathParticles.Play();
        _audio.Stop();
        _audio.PlayOneShot(_deathSound);
        Invoke("ResetGame", _transcendingDelay);
    }

    private void Thrust()
    {
        // Thrust
        HandleMainEngineSound();
        if (Input.GetKey(KeyCode.Space))
        {
            _rocket.AddRelativeForce(Vector3.up * _mainThrustSpeed * Time.deltaTime);
            _mainEngineParticles.Play();
            return;
        }

        _mainEngineParticles.Stop();
    }

    private void HandleMainEngineSound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audio.PlayOneShot(_mainEngineAudio);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _audio.Pause();
        }
    }

    private void Rotate()
    {
        // Rotate
        _rocket.freezeRotation = true; // To take manual control, and prevent exessive rotation when hitting something

        // Right
        var rotationForThisFrame = _rotationSpeed * Time.deltaTime;
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

    #region Invokable Events
    private void LoadNextScene()
    {
        var lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        SceneManager.LoadScene(CurrentSceneIndex == lastSceneIndex ? 0 : CurrentSceneIndex + 1);
    }
    private void ResetGame() => SceneManager.LoadScene(CurrentSceneIndex);
    #endregion
}
