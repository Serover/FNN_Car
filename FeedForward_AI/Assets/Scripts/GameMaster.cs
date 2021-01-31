using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [Header("GAME")]
    [SerializeField] public int laps;
    [HideInInspector] public int playerLaps;
    [HideInInspector] public int _AILaps;
    [HideInInspector] public bool gamePlaying;

    [SerializeField] private GameObject finishLine;

    [Header("Car")]
    public GameObject carGameObj;
    private GameObject aiCar;
    public CarMovement car;
    public Sensors sensors;
    [Range(0, 1)] public float gas = 0;
    [Range(0, 1)] public float turn = 0;
    private float[,] correctOutPut;

    // Private stuff
    private RaycastHit2D[][] arrayOfSensorArray;
    private float[][] distances;
    private float[] weigths;
    private float[] gasPoints;
    private float[] turnPoints;


    private bool AIPlaying = false;
    private bool hasRecording = false;

    [Header("Recording")]
    public bool recording;
    public int recordingTicks;
    private int recordingLoop;

    [Header("Training")]
    public float trainingRuns;
    public float learningRate;
    private bool hasTrained = false;
    private float[] gasWeigths;
    private float[] turnWeigths;

    [Header("BackWordsProp")]
    private int[] layers;
    private const int outPutSize = 2;
    [SerializeField] private int[] middleLayers;
    [SerializeField] private int bias;
    NeuralNetwork neuralNetwork;
    void Start()
    {
        arrayOfSensorArray = new RaycastHit2D[recordingTicks][];
        gasPoints = new float[recordingTicks];
        turnPoints = new float[recordingTicks];
        distances = new float[recordingTicks][];
        correctOutPut = new float[recordingTicks, 2];

        FixLayerArray();

        neuralNetwork = new NeuralNetwork(layers, learningRate, bias);
    }

    private void FixLayerArray()
    {
        List<int> temp = new List<int>();
        temp.Add(sensors.nrOfRays * 3);
        for (int i = 0; i < middleLayers.Length; i++)
        {
            temp.Add(middleLayers[i]);
        }
        temp.Add(outPutSize);

        layers = temp.ToArray();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !recording)
        {
            Debug.Log("Recording");

            recording = true;
            recordingLoop = 0;
        }

        if (recording)
            Recording();

        if (hasRecording && !hasTrained && Input.GetKeyDown(KeyCode.T))
        {
            AItrain();
            Debug.Log("Training");
        }

        if (!AIPlaying && Input.GetKeyDown(KeyCode.P) && hasTrained)
        {
            GameObject car = Instantiate(carGameObj, new Vector3(-4, -4, 0), Quaternion.identity);
            aiCar = car;
            // instantiate new car object
            // reset pos på My Car
            sensors.gameObject.transform.position = new Vector3(-4, -3, 0);
            // 
            AIPlaying = true;
            gamePlaying = true;
        }

    }
    private void FixedUpdate()
    {
        if (AIPlaying && hasTrained)
        {
            AIplay(aiCar);
        }


        PlayerPlay();

    }

    void Recording()
    {
        if (recordingLoop < recordingTicks)
        {
            float[] distanceTick = sensors.FetchDistances();

            distances[recordingLoop] = distanceTick;
            correctOutPut[recordingLoop, 0] = gas;
            correctOutPut[recordingLoop, 1] = turn;

            recordingLoop++;
        }
        else
        {
            recording = false;
            hasRecording = true;

            Debug.Log("Recording Done");
        }
    }
    void PlayerPlay()
    {
        if (Input.GetMouseButton(1))
            gas = 1;
        else
            gas = 0;

        turn = car.MouseToTurn();
        car.DoCarMovement(gas, turn);
    }
    void AItrain()
    {
        for (int j = 0; j < trainingRuns; j++)
        {
            for (int i = 0; i < recordingTicks; i++)
            {
                neuralNetwork.FeedForward(distances[i]);

                float[] temp = new float[2];
                temp[0] = correctOutPut[i, 0];
                temp[1] = correctOutPut[i, 1];

                neuralNetwork.BackPropStart(temp);
            }
            hasTrained = true;
        }
        Debug.Log("Training Done");
    }
    void AIplay(GameObject car)
    {
        Debug.Log("fetchdistance size " + car.GetComponent<Sensors>().FetchDistances().Length);

        float[] output = neuralNetwork.FeedForward(car.GetComponent<Sensors>().FetchDistances());
    
        car.GetComponent<CarMovement>().DoCarMovement(output[0], output[1]);

        gas = output[0];
        turn = output[1];
    }
}
