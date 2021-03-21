using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private Vector3 initPos, pos;
    public Transform prefab;
    [HideInInspector] public List<MovementController> cars = new List<MovementController>();
    private List<bool> giveSpeed;
    public float distance = 3.68F;
    public float startDistance = -3.68F;
    public float carNumber = 4F;
    public float lowspeed1 = 7f, maxspeed1 = 10f;
    
    void Start()
    {
        giveSpeed = new List<bool>();
        Random.InitState(System.DateTime.Now.Millisecond);
        float lane1 = -1F;
        float lane3 = -1F;
        float lane2 = -1F;
        if(carNumber%3 == 2)
        {
            for (int i = 0; i < carNumber; i++)
            {
                //Random.InitState(i);
                if(i < carNumber/3){
                    lane1 = lane1+1F;
                    CreateCar(0,lane1,lowspeed1,maxspeed1);

                }
                else if(i >= carNumber/3+1 && i < (carNumber/3)*2)
                {
                    lane2 = lane2+1F;
                    CreateCar(1,lane2,lowspeed1,maxspeed1);
                }
                else{
                    lane3 = lane3+1F;
                    CreateCar(2,lane3,lowspeed1,maxspeed1);   
                    }
                    cars[cars.Count-1].name = i.ToString();
                    
            }

        }
        else{

            for (int i = 0; i < carNumber; i++)
            {
                //Random.InitState(System.DateTime.Now.Millisecond);
                if(i < carNumber/3){
                    lane1 = lane1+1F;
                    CreateCar(0,lane1,lowspeed1,maxspeed1);
                }
                else if(i >= carNumber/3+1 && i < (carNumber/3)*2+1)
                {
                    lane2 = lane2+1F;
                    CreateCar(1,lane2,lowspeed1,maxspeed1);
                }
                else{
                    lane3 = lane3+1F;
                    CreateCar(2,lane3,lowspeed1,maxspeed1);
                }
                cars[cars.Count-1].name = i.ToString();
                    
            }
        }

        
        StartCoroutine("carsAIspeed");
        StartCoroutine("carsAItumble");
        
    }

    void Update() {
        if(!GameManager.begin)
            return;
            
        DetectBlock();
        for (int i = 0; i < cars.Count; ++i)
            if(giveSpeed[i])
                cars[i].IncreaseSpeed();
    }

    void DetectBlock() {
        for (int i = 0; i < cars.Count; ++i)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                new Vector2(cars[i].transform.position.x, cars[i].transform.position.y),
                Vector2.right, 
                3f, 
                cars[0].carLayermask);

            if(hit.collider == null)
                continue;

            giveSpeed[i] = false;
            int rnd = Random.Range(1,3);
            if(rnd == 1){
                if(!cars[i].MoveLaneUp())
                    cars[i].MoveLaneDown();
            }
            else 
                if(!cars[i].MoveLaneDown())
                    cars[i].MoveLaneUp();
                
            
        }
    }

    IEnumerator carsAIspeed()
    {
        yield return new WaitForSeconds(10f); 
        while(true)
        {
            int x = Random.Range (0, cars.Count);
            for(int i = 0; i < giveSpeed.Count; ++i)
                giveSpeed[i] = true;
            giveSpeed[x] = false;
            yield return new WaitForSeconds(.5f); 
        }
        
    }

    IEnumerator carsAItumble()
    {
        yield return new WaitForSeconds(7f); 
        while(true)
        {
            int x = Random.Range (0, cars.Count);
            cars[x].Tumble();
            yield return new WaitForSeconds(5f); 
        }
        
    }

    void CreateCar(int lane, float constant, float lowspeed, float maxspeed){
        if(lane == 1){
            Transform car = Instantiate(prefab, new Vector3((startDistance+distance)+ distance*2*constant, Lanes.height[lane], Lanes.height[lane]), Quaternion.identity);
            cars.Add(car.GetComponent<MovementController>());
            cars[cars.Count-1].maxSpeed = GenerateRandomNumber(lowspeed,maxspeed);
        }
        else{
        Transform car = Instantiate(prefab, new Vector3(startDistance+ distance*2*constant, Lanes.height[lane], Lanes.height[lane]), Quaternion.identity);              
        cars.Add(car.GetComponent<MovementController>());
        cars[cars.Count-1].maxSpeed = GenerateRandomNumber(lowspeed,maxspeed);
        }
        cars[cars.Count-1].lane = lane;
        giveSpeed.Add(true);
    }

    private List<float> lastNumbers =  new List<float>();

    public float GenerateRandomNumber(float x, float y)
    {
        //The highest number you want here. The lower number is inclusive, higher one exclusive.
        float rand = Random.Range(x, y);
        int nrTries = 20;
        //Regenerate while the lastNumbers contains random.
        while (lastNumbers.Contains(rand) && nrTries > 0)
        {
            rand = Random.Range(x, y);
            nrTries--;
        }

        //Store it.
        AddNumberToList(rand);

        //Give back the number we generated.
        return rand;
    }

    void AddNumberToList(float number)
    {
        if (lastNumbers.Count > 3)
        {
            lastNumbers.RemoveAt(lastNumbers.Count - 1);
        }
        lastNumbers.Insert(0, number);
    }

}
