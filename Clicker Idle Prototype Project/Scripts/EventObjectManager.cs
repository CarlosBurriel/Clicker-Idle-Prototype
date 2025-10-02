using System.Collections;
using UnityEngine;

public class EventObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject eventObject;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private int startCooldown; //Time to start the first event
    [SerializeField] public int eventCooldown; //Max Time for an event to begin

    private GameObject eventObjectAux;
    private Vector2 randomPosition;
    private Vector3 direction;
    private bool cooldownFinished = true;

    void Update()
    {
        if (cooldownFinished)
            StartCoroutine(EventCooldown());

        if(eventObjectAux != null)
        {
            eventObjectAux.transform.Translate(direction * Time.deltaTime * speed);
        }
    }

    IEnumerator EventCooldown()
    {
        cooldownFinished = false;
        if(startCooldown >= eventCooldown) { eventCooldown = startCooldown + 1; }
        yield return new WaitForSeconds(Random.Range(startCooldown, eventCooldown));
        SpawnEventObject();

        yield return new WaitForSeconds(30f);
        Destroy(eventObjectAux);
        eventObjectAux = null;
        cooldownFinished = true;
    }

    private void SpawnEventObject()
    {
        randomPosition = new Vector2(Random.Range(1080, 1100), Random.Range(-630, 630));
        eventObjectAux = Instantiate(eventObject, new Vector3(randomPosition.x, randomPosition.y, 0), new Quaternion(0,0,0, 1), transform);
        direction = target.position - eventObjectAux.transform.position;
    }
}
