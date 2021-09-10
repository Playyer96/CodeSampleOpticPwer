using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checklist : MonoBehaviour
{
    private static List<Checklist> all;

    public string Name;
    [SerializeField]
    internal List<ChecklistItem> items;

    public bool IsChecked { get { return items.FindAll(i => i.required && i.isChecked).Count == items.FindAll(i => i.required).Count && items.FindAll(i => i.required).Count > 0; } }
    public UnityEvent OnFullChecked;

    //public Checklist() : this(new Vector3(3f, 3f, 3f))
    //{
    //    Debug.Log("First????");
    //}

    //public Checklist(Vector3 v)
    //{
    //    Debug.Log("Created: " + v);
    //}

    private void OnEnable()
    {
        if (all == null)
            all = new List<Checklist>();
        if (Name != "")
            all.Add(this);
    }

    private void OnDisable()
    {
        Debug.Log("Removed checklist: " + all.RemoveAll(c => c == this).ToString());
    }

    //private void Awake()
    //{
    //    if (all == null)
    //        all = new List<Checklist>();
    //    if (Name != "")
    //        all.Add(this);
    //}

    public void CheckedOn(string itemName)
    {
        //if (items.FindAll(i => i.name == itemName).Count == 0)
        //    items.Add(new ChecklistItem() { name = itemName, isChecked = true, required = required });
        //else
        items.FindAll(i => i.name == itemName).ForEach(i => { i.isChecked = true; });
        if (IsChecked)
            OnFullChecked.Invoke();
    }

    public void CheckedOff(string itemName)
    {
        items.FindAll(i => i.name == itemName).ForEach(i => { i.isChecked = false; });
    }

    public void Set(string itemName, bool val, bool required = true)
    {
        if (!Application.isPlaying)
            return;
        //if (IsChecked && val)//QUITAR COMENTARIO?
        //    return;

        if (items.FindAll(i => i.name == itemName).Count == 0)
        {
            Debug.LogError("The item \"" + itemName + "\" does not exists in checklist \"" + Name + "\"!");

            //ChecklistItem item = new ChecklistItem() { name = itemName, isChecked = val, required = required, OnCheckedChanged = new ChecklistItem.CheckedChanged() { } };
            //item.OnCheckedChanged.AddListener(delegate (bool v) { Debug.Log("Checked: " + v.ToString()); });
            //items.Add(item);
        }
        else
        {
            items.FindAll(i => i.name == itemName).ForEach(i =>
            {
                if (i.delay > 0f)
                {
                    Debug.Log("DELAY!!");
                    StartCoroutine(i.Delayed(i.delay, i.OnCheckedChanged, val));
                }
                else
                    i.isChecked = val;
            });
        }
        if (IsChecked)
            OnFullChecked.Invoke();
    }

    public static void Set(string listName, string itemName, bool val, bool required = true)
    {
        if (!Application.isPlaying)
            return;
        //Debug.LogFormat("{0} - {1} - {2}", listName, itemName, val.ToString());
        Checklist checklist = all.Find(c => c.Name == listName);
        if (checklist == null)
        {
            //checklist = new Checklist() { Name = listName };
            //all.Add(checklist);

            //checklist = new GameObject("Checklist [" + listName + "]", typeof(Checklist)).GetComponent<Checklist>();
            //checklist.OnFullChecked = new UnityEvent();
            //checklist.Name = listName;
            //checklist.items = new List<ChecklistItem>();
            //all.Add(checklist);
        }
        if (checklist != null)
            checklist.Set(itemName, val, required);
    }

    //public static void Set(string listName, string itemName, bool val, bool required, ChecklistItem.CheckedChanged onCheckedChanged)
    public static void AddOrUpdate(string listName, string itemName, bool val, bool required, UnityAction<bool> action = null)
    {
        if (!Application.isPlaying)
            return;
        //if (IsChecked && val)
        //    return;

        Checklist checklist = all.Find(c => c.Name == listName);

        if (checklist == null)
        {
            Debug.LogError("The checklist \"" + listName + "\" does not exists!");
            return;
        }
        ChecklistItem.CheckedChanged cc = new ChecklistItem.CheckedChanged();
        if (action != null)
            cc.AddListener(action);
        ChecklistItem item = new ChecklistItem() { name = itemName, isChecked = val, required = required, OnCheckedChanged = cc };
        checklist.items.Add(item);
    }

    public static void Remove(string listName, string itemName)
    {
        if (!Application.isPlaying)
            return;

        Checklist checklist = all.Find(c => c.Name == listName);

        if (checklist == null)
        {
            Debug.LogError("The checklist \"" + listName + "\" does not exists!");
            return;
        }

        int removed = checklist.items.RemoveAll(i => i.name == itemName);
        Debug.Log(removed + " checklist items removed!");
    }

    public static void ClearItems(string listName)
    {
        if (!Application.isPlaying)
            return;

        Checklist checklist = all.Find(c => c.Name == listName);

        if (checklist == null)
        {
            Debug.LogError("The checklist \"" + listName + "\" does not exists!");
            return;
        }
        checklist.items.Clear();
    }

    public bool Get(string itemName)
    {
        if (!Application.isPlaying)
            return false;
        if (items.FindAll(i => i.name == itemName).Count > 0)
        {
            return items.FindAll(i => i.isChecked && i.name == itemName).Count == items.FindAll(i => i.name == itemName).Count;
        }
        else
        {
            //items.Add(new ChecklistItem() { name = itemName, isChecked = false, required = true });
            return false;
        }
    }

    public static bool Get(string listName, string itemName = "")
    {
        if (!Application.isPlaying)
            return false;
        if (itemName.Trim() != "")
        {
            Checklist checklist = all.Find(c => c.Name == listName);
            if (checklist == null)
                Debug.LogErrorFormat("The checklist \"{0}\" does not exist!", listName);

            if (itemName.Trim() == "")
                return checklist == null ? false : checklist.IsChecked;
            else
                return checklist == null ? false : checklist.Get(itemName.Trim());
        }
        else
        {
            Checklist checklist = all.Find(c => c.Name == listName);
            return checklist == null ? false : checklist.IsChecked;
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Removed checklist: " + Checklist.all.RemoveAll(c => c == this).ToString());
    }
}

[Serializable]
public class ChecklistItem
{
    public string name;
    //[Multiline]
    //public string comments;
    public bool isChecked
    {
        get => IsChecked; set
        {
            if (value != IsChecked)
            {
                if (OnCheckedChanged == null)
                    OnCheckedChanged = new CheckedChanged();
                OnCheckedChanged.Invoke(value);
                //OnCheckedChanged.Invoke(negateValue ? !value : value);
            }
            IsChecked = value;
        }
    }

    [NonSerialized]
    private bool IsChecked;
    /// <summary>
    /// true if this item is required for global check
    /// </summary>
    public bool required = true;

    //[Tooltip("Negate the value given to OnCheckedChanged")]
    //public bool negateValue;
    public float delay;
    public CheckedChanged OnCheckedChanged;
    //private MonoBehaviour mb = new MonoBehaviour();
    [Serializable]
    public class CheckedChanged : UnityEvent<bool> { }
    public IEnumerator Delayed(float delay, CheckedChanged ev, bool val)
    {
        IsChecked = val;
        Debug.Log("DELAYED: " + val.ToString());
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        ev.Invoke(val);
    }
}
