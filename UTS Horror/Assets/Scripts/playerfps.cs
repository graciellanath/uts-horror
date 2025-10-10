using UnityEngine;

public class playerfps : MonoBehaviour
{
    float minimumY = -60f;
    float maximumY = 60f;
    float speedPutar = 5f;

    float rotationY = 0f;

    void Update()
    {
        //perputaran hero & camera berdasarkan pointer mouse
        //putar hero horisontal
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * speedPutar;
        this.transform.localEulerAngles = new Vector3(0, rotationX, 0);

        //putar camera vertical
        rotationY += Input.GetAxis("Mouse Y") * speedPutar;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Camera.main.transform.localEulerAngles = new Vector3(-rotationY,0,0);

        //senjata perlu diputar?
        this.transform.Find("Cube").transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

        //Gerakkan maju mundur karakter (caranya biasa)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        this.transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * 5f);
    }
}
