using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
    {
    #region Singleton
    public static Inventory instance;
    void Awake()
        {
        if(instance == null)
            instance = this;
        }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySpace = 40;
    public List<Item> items = new List<Item>();
    public bool Add (Item item)
        {
        if (items.Count >= inventorySpace) return false;
        items.Add(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
        }
    public void remove(Item item)
        {
        items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        }
    }
