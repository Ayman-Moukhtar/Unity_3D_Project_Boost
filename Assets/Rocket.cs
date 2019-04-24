using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody _rocket;

    // Start is called before the first frame update
    void Start()
    {
        _rocket = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rocket.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rocket.transform.Rotate(Vector3.back);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rocket.transform.Rotate(Vector3.forward);
        }
    }
}
