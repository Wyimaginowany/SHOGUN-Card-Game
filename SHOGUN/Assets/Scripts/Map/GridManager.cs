using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [Header("Generator Settings")]
    [SerializeField] private int _stages;
    [SerializeField] private int _locations;
    [SerializeField] private int _canvasStartX=170,_canvasStartY=200;
    [SerializeField] [Range(0,1)] private float _maxOffsetX=(float)0.6,_maxOffsetY=(float)0.6;
    

    [SerializeField] private MapEvent _combatEvent;
    [SerializeField] private Canvas _canvas;
    private Dictionary<Vector2,MapEvent> _allEvents;
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

    void GenerateGrid(){
        _allEvents=new Dictionary<Vector2, MapEvent>();
        Dictionary<Vector2, MapEvent> previousStageEvents=new();
        Dictionary<int,List<MapEvent>> newStageEvents=new();
        List<int> newEventsLocations=new();
        var eventTileWidth= (int)(_canvas.GetComponent<RectTransform>().rect.width-_canvasStartX*2)/_stages;
        var eventTileHeight= (int)(_canvas.GetComponent<RectTransform>().rect.height-_canvasStartY*2)/_locations;

        
        for(int stage=0;stage<_stages;stage++){
            if(stage==0){
                newEventsLocations=GenerateEntryLocations();
            }else{
                newStageEvents=GenerateNewStageEvents(previousStageEvents);
                newEventsLocations=newStageEvents.Keys.ToList();
            }

            previousStageEvents.Clear();

            for (int location=0; location<_locations;location++){
                if(newEventsLocations.Contains(location)){
                    var spawnedEvent=Instantiate(_combatEvent,transform,false);
                    spawnedEvent.name=$"Event {stage}-{location}";

                    var offset=GenerateEventPosition(eventTileWidth,eventTileHeight,stage,location);
                    spawnedEvent.transform.localPosition=new Vector3(_canvasStartX+offset.x,_canvasStartY+offset.y);

                    _allEvents.Add(new Vector2(stage,location),spawnedEvent);
                    previousStageEvents.Add(new Vector2(stage,location),spawnedEvent);
                    
                    if(newStageEvents.ContainsKey(location)){
                        foreach(MapEvent mapEvent in newStageEvents[location]){
                           spawnedEvent.GetComponent<MapEvent>().setParentEvent(mapEvent);
                           mapEvent.GetComponent<MapEvent>().addChildrenEvent(spawnedEvent);
                        }
                    }
                }
            }

            if(stage==0){
                foreach(MapEvent startEvent in previousStageEvents.Values){
                    startEvent.GetComponent<Collider2D>().enabled=true;
                    startEvent.GetComponent<MapEvent>().ImportEnabledEvents(previousStageEvents.Values.ToList());
                }
            }
        }
    }


    private List<int> GenerateEntryLocations(){
        var firstRoom=Random.Range(0,_locations);
        var secondRoom=Random.Range(0,_locations);
        while(secondRoom==firstRoom){
            secondRoom=Random.Range(0,_locations);
        }
        var entryRooms= new List<int>
        {
            firstRoom,
            secondRoom
        };
        return entryRooms;
    }


    private Vector3 GenerateEventPosition(int tileWidth, int tileHeight, int stage, int location){
        var offsetX=Random.Range(0,tileWidth*_maxOffsetX);
        var offsetY=Random.Range(0,tileHeight*_maxOffsetY);
        return new Vector3(tileWidth*stage+offsetX,tileHeight*location+offsetY);
    }


    private Dictionary<int,List<MapEvent>> GenerateNewStageEvents(Dictionary<Vector2, MapEvent> previousStageEvents){
        Dictionary<int,List<MapEvent>> newEventsList=new();
        List<int> eventLocations= new();
        Tuple<List<int>,int> events;
        

        foreach (var prevEvent in previousStageEvents){
            int paths = 0;
            eventLocations.Clear();

            events=CreateEvents(prevEvent.Key.y);
            paths=events.Item2;
            eventLocations=events.Item1;
            
            foreach(int location in eventLocations){
                if(newEventsList.ContainsKey(location)){
                    newEventsList[location].Add(prevEvent.Value);
                }else{
                    newEventsList[location]= new List<MapEvent>
                    {
                        prevEvent.Value
                    };
                }
            }
        }
        return newEventsList;
    }

    private Tuple<List<int>,int> CreateEvents(float y){
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
        return Tuple.Create(eventLocations,paths);
    }


    
}
