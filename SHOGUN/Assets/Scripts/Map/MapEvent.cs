using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEvent : MonoBehaviour, IPointerClickHandler
{
    private List<MapEvent> _childrenEvents,_parentEvents,_enabledEvents;
    public RectTransform _rectTransform;

    [SerializeField] private LineController _line;
    [SerializeField] private Sprite _visited;
    [SerializeField] private Image _imageControler;

    public static event Action OnNewStageStarted;

    
    public void OnPointerClick(PointerEventData eventData)
    {
       UpdateMap();
       OpenScene();
    }

    private void Awake() {
        _parentEvents=new List<MapEvent>();
        _childrenEvents=new List<MapEvent>();
    }

    public void setParentEvent(MapEvent parentEvent){
        _parentEvents.Add(parentEvent);
        
        LineController newLine=Instantiate(_line,transform);
        newLine.setTarget(parentEvent.transform,transform);
    }


    public void addChildrenEvent(MapEvent child){
        _childrenEvents.Add(child);
    }

    private void UpdateMap(){
        // gameObject.GetComponent<UnityEngine.UI.Image>().overrideSprite=_visited;
        _imageControler.sprite=_visited;

        foreach (MapEvent enabled in _enabledEvents){
            enabled.GetComponent<Image>().raycastTarget=false;
        }
        _enabledEvents.Clear();

        _enabledEvents.AddRange(_childrenEvents);
        foreach (MapEvent child in _childrenEvents){
            child.GetComponent<Image>().raycastTarget=true;
            child.GetComponent<MapEvent>().ImportEnabledEvents(_enabledEvents);

        }
    }


    public void ImportEnabledEvents(List<MapEvent> events){
        _enabledEvents=new List<MapEvent>(events);
    }

   


    public void OpenScene()
    {
        MapObject.MapInstance.GetComponent<MapObject>().HideMap();
        OnNewStageStarted?.Invoke();
    }
}
