using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using Microsoft.Unity.VisualStudio.Editor;

public class MapEvent : MonoBehaviour, IPointerClickHandler
{
    private List<MapEvent> _childrenEvents,_parentEvents,_enabledEvents;
    public RectTransform _rectTransform;

    [SerializeField] private LineController _line;
    [SerializeField] private Sprite _visited;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Image _imageControler;

    
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
        _spriteRenderer.sprite=_visited;

        foreach (MapEvent enabled in _enabledEvents){
            enabled.GetComponent<BoxCollider2D>().enabled=false;
        }
        _enabledEvents.Clear();

        _enabledEvents.AddRange(_childrenEvents);
        foreach (MapEvent child in _childrenEvents){
            child.GetComponent<BoxCollider2D>().enabled=true;
            child.GetComponent<MapEvent>().ImportEnabledEvents(_enabledEvents);

        }
    }


    public void ImportEnabledEvents(List<MapEvent> events){
        _enabledEvents=new List<MapEvent>(events);
    }

   


    public void OpenScene()
    {
        MapObject.MapInstance.GetComponent<MapObject>().HideMap();
        SceneManager.LoadScene("Scene 1");
    }
}
