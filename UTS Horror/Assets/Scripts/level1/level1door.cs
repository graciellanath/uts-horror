using UnityEngine;

public class level1door : MonoBehaviour
{
    Vector3 localAwal;
    Vector3 localBuka;

    GameObject hero;

    void Start()
    {
        // posisi pintu di local space
        localAwal = transform.localPosition;

        // geser -X lokal untuk "ke kiri"
        localBuka = localAwal + new Vector3(-3f, 0, 0);

        hero = GameObject.Find("hero");
    }

    void Update()
    {
        float jarakHero = Vector3.Distance(hero.transform.position, transform.position);

        if (jarakHero < 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localBuka, Time.deltaTime * 2f);
        }
        else if (jarakHero > 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localAwal, Time.deltaTime * 2f);
        }
    }
}
