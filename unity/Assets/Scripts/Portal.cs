using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Portal : MonoBehaviour {
    public float CoolDown = 15;
    public float KeyDropRate = 0.7f;
    public float BursterDropRate = 4;

    public FloatingText FloatingDamageTextPrefab;
    public FloatingText FloatingRechargeTextPrefab;

    public PortalModel Model { get; private set; }
    public float Energy { get; private set; }
    public AchievementModel DestroyerAchievement { get; private set; }

    private GameObject KeySprite;
    private GameObject PortalSprite;
    private GameObject UncapturedPortalSprite;
    private TimeTicker coolDownTimer;

    public Portal Init(PortalModel model)
    {
        Model = model;
        return this;
    }

	// Use this for initialization
	void Start () {
        KeySprite = transform.Find("Key").gameObject;
        PortalSprite = transform.Find("Portal").gameObject;
        UncapturedPortalSprite = transform.Find("UncapturedPortal").gameObject;
        coolDownTimer = new TimeTicker(CoolDown);

        transform.position = transform.position.Set2D(Model.Position);

        SetHasKey(false);
        UpdateFaction();
        ResetEnergy();
    }

    private void UpdateFaction()
    {
        if (Model.Faction != Faction.None)
        {
            PortalSprite.SetActive(true);
            UncapturedPortalSprite.SetActive(false);
            PortalSprite.GetComponent<Renderer>().material = FactionManager.GetInstance().GetSolidMaterial(Model.Faction);
        } else
        {
            PortalSprite.SetActive(false);
            UncapturedPortalSprite.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
    }

    // TODO Player specific?
    public bool TryHack(Faction faction, ItemInventoryModel inventory)
    {
        if (!coolDownTimer.IsTimeout())
        {
            return false;
        }

        coolDownTimer.Start();

        inventory.AddBursters(Mathf.FloorToInt(Random.value * BursterDropRate));

        if (Random.value < KeyDropRate)
        {
            inventory.AddKey(Model);
        }

        return true;
    }

    public void SetHasKey(bool hasKey)
    {
        KeySprite.SetActive(hasKey);
    }

    public void Capture(Faction faction)
    {
        Model.Faction = faction;
        UpdateFaction();

        if (faction != Faction.None)
        {
            ResetEnergy();
            DestroyerAchievement = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var burst = collision.GetComponent<Burst>();
        if (burst != null)
        {
            ReceiveDamage(burst);
        }
    }

    private void ReceiveDamage(Burst burst)
    {
        if (Model.Faction != Faction.None && burst.Faction != Model.Faction)
        {
            var distance = (Model.Position - burst.transform.position.To2D()).magnitude;
            var damage = 1 + (75 - Model.Mitigation) / (distance + 1) / 2;
            Debug.Log("Portal " + Model + "receiving damage " + damage + " from distance " + distance + " with mitigation " + Model.Mitigation);
            ReduceEnergy(damage);

            if (Energy == 0)
            {
                DestroyerAchievement = burst.OwnerAchievement;

                if (burst.Faction == Faction.Player)
                {
                    SoundManager.GetInstance().PortalDestroyedSound.Play();
                }
            }
        }
    }

    private void ResetEnergy()
    {
        Energy = 100;
    }

    private void ReduceEnergy(float value)
    {
        Util.Instantiate(FloatingDamageTextPrefab, null).
            Init(Model.Position, Mathf.Floor(value).ToString(), 0.5f);

        Energy = Mathf.Clamp(Energy - value, 0, 100);
    }

    public void RechargeEnergy(float value)
    {
        Util.Instantiate(FloatingRechargeTextPrefab, null).
            Init(Model.Position, "+" + Mathf.Floor(value).ToString(), 0.5f);

        Energy = Mathf.Clamp(Energy + value, 0, 100);
    }

}
