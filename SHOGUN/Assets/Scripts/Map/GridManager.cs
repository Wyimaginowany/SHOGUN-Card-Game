using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [Header("Generator Settings")]
    [SerializeField] private int _stages; //stage- event x value
    [SerializeField] private int _locations; //location- event y value
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _canvasStartX=170,_canvasStartY=200; //canvas size
    [SerializeField] [Range(0,1)] private float _maxOffsetX=(float)0.6,_maxOffsetY=(float)0.6;//max potential shift of event position on canvas
    [SerializeField] private MapEvent _combatEvent,_treasureEvent,_campfireEvent; //types of events
    [SerializeField] [Range(0,1)] private float _combatChance=(float)0.6,_treasureChance=(float)0.2,_campfireChance=(float)0.2;//the chances of drawing a specific type of event in a given location
    private Dictionary<Vector2,MapEvent> _allEvents;
    private Dictionary<MapEvent,ValueTuple<float,string>> _eventTypes;
    public static GameObject MapInstance;

    void Start()
    {
        GenerateGrid();
    }
    private void Awake() {
        if (MapInstance != null){
            Destroy(gameObject);
        }else{
            
            MapInstance=gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }


    //Generate Grid-based map of connected events
    void GenerateGrid(){
        _allEvents=new Dictionary<Vector2, MapEvent>();

        //connecting item types with their weight
        _eventTypes=new Dictionary<MapEvent,ValueTuple<float,string>>(){
            {_combatEvent,(_combatChance,"Combat")},
            {_treasureEvent,(_treasureChance,"Treasure")},
            {_campfireEvent,(_campfireChance,"Campfire")}
        };
        Dictionary<Vector2, MapEvent> previousStageEvents=new();
        Dictionary<int,List<MapEvent>> newStageEvents=new();
        List<int> newStageEventsLocations=new();
        var eventTileWidth= (int)(_canvas.GetComponent<RectTransform>().rect.width-_canvasStartX*2)/_stages;
        var eventTileHeight= (int)(_canvas.GetComponent<RectTransform>().rect.height-_canvasStartY*2)/_locations;

        
        for(int stage=0;stage<_stages;stage++){
            if(stage==0){
                newStageEventsLocations=GenerateEntryLocation();
            }else{
                newStageEvents=GenerateNewStageEvents(previousStageEvents);
                newStageEventsLocations=newStageEvents.Keys.ToList();
            }

            previousStageEvents.Clear();

            for (int location=0; location<_locations;location++){
                if(newStageEventsLocations.Contains(location)){
                    ValueTuple<MapEvent,string> drawnEventType;

                    //Drawing type of event
                    if(stage==0||stage==_stages-1){
                        drawnEventType=(_combatEvent,"Combat");
                    }else{

                        //checks if parent event wasn't combat
                        bool drawningEvent=true;
                        foreach(MapEvent mapEvent in newStageEvents[location]){
                            if(mapEvent._eventType=="Treasure"||mapEvent._eventType=="Campfire")
                                drawningEvent=false;
                        }
                        if(drawningEvent) drawnEventType=EventTypeSelector();
                        else drawnEventType=(_combatEvent,"Combat");
                    }


                    //Creating event
                    var spawnedEvent=Instantiate(drawnEventType.Item1,transform,false);
                    spawnedEvent.name=$"Event {stage}-{location}";
                    spawnedEvent.setEventType(drawnEventType.Item2);


                    //drawing shift of event position on canvas
                    var offset=GenerateEventPosition(eventTileWidth,eventTileHeight,stage,location);
                    spawnedEvent.transform.localPosition=new Vector3(_canvasStartX+offset.x,_canvasStartY+offset.y);


                    //adding event to list of all events and to previous stage list for next stage generating process
                    _allEvents.Add(new Vector2(stage,location),spawnedEvent);
                    previousStageEvents.Add(new Vector2(stage,location),spawnedEvent);
                    

                    //Skips parent assigning process if created event is entry location
                    if(newStageEvents.ContainsKey(location)){
                        //assigning parents to child event in given location
                        foreach(MapEvent mapEvent in newStageEvents[location]){
                           
                           spawnedEvent.GetComponent<MapEvent>().setParentEvent(mapEvent);
                           mapEvent.GetComponent<MapEvent>().addChildrenEvent(spawnedEvent);
                        }
                    }
                }
            }
            //Enabling first event
            if(stage==0){
                foreach(MapEvent startEvent in previousStageEvents.Values){
                    startEvent.GetComponent<Image>().raycastTarget=true;
                    startEvent.setEventType("Combat");
                    startEvent.GetComponent<MapEvent>().ImportEnabledEvents(previousStageEvents.Values.ToList());
                }
            }
        }
    }

    //Entry Event Location
    private List<int> GenerateEntryLocation(){
        var firstRoom=_locations/2;
        // var secondRoom=Random.Range(0,_locations);
        // while(secondRoom==firstRoom){
        //     secondRoom=Random.Range(0,_locations);
        // }
        var entryRooms= new List<int>
        {
            firstRoom
            // secondRoom
        };
        return entryRooms;
    }

    //Draws position in location of event that is being created
    private Vector3 GenerateEventPosition(int tileWidth, int tileHeight, int stage, int location){
        var offsetX=Random.Range(0,tileWidth*_maxOffsetX);
        var offsetY=Random.Range(0,tileHeight*_maxOffsetY);
        return new Vector3(tileWidth*stage+offsetX,tileHeight*location+offsetY);
    }

    //Creates dict that contains [location of the new child event, its parents]  
    private Dictionary<int,List<MapEvent>> GenerateNewStageEvents(Dictionary<Vector2, MapEvent> previousStageEvents){
        Dictionary<int,List<MapEvent>> newStageEventsLocations=new();
        List<int> childEventsLocations= new();

        foreach (var parentEvent in previousStageEvents){
            childEventsLocations.Clear();
            childEventsLocations=CreateEvents(parentEvent.Key.y);
            
            //assigning parents to given location
            foreach(int location in childEventsLocations){
                if(!newStageEventsLocations.ContainsKey(location)){
                    newStageEventsLocations[location]= new List<MapEvent>
                    {
                        parentEvent.Value
                    };
                }else{
                    newStageEventsLocations[location].Add(parentEvent.Value);
                }
            }
        }
        return newStageEventsLocations;
    }


    //Draws amount of child events and their locations
    private List<int> CreateEvents(float y){
        List<int> eventLocations= new();
        int paths=0;
        while(paths<1){
            if(y+1<_locations&&Random.Range(0,2)==1){
                eventLocations.Add((int)y+1);
                paths++;
            }
            if(y-1>=0&&Random.Range(0,2)==1){
                eventLocations.Add((int)y-1);
                paths++;
            }
          
            if(paths<2&&Random.Range(0,2)==1){
                eventLocations.Add((int)y);
                paths++;
            }
        }
        return eventLocations;
    }

    
    //Selecting random event choosed by Weighted Random Algorithm
    private ValueTuple<MapEvent,string> EventTypeSelector(){
        float totalWeight=0;
        Dictionary<MapEvent,ValueTuple<float,string>> eventBag=new Dictionary<MapEvent,ValueTuple<float,string>>();
        foreach (var type in _eventTypes){
            totalWeight+=type.Value.Item1;
            eventBag.Add(type.Key,(totalWeight,type.Value.Item2));
        }
        float r=Random.Range(0,totalWeight);
        foreach (var item in eventBag){
            if(item.Value.Item1>=r){
                return (item.Key,item.Value.Item2);
            }
        }
        return default(ValueTuple<MapEvent,string>);
    }


    
}
