using ChronoArkMod.Helper;
using EEx.Common;
using HarmonyLib;
using I2.Loc;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EEx.Implementation.Temp;

public class MonsterDataDisplay : UIBehaviour
{
    private const float BasicHeightAdjust = 60f;

    private static Sprite? _turnIcon;
    private static bool _lookupOnce;
    private GameObject? _align;
    private TextMeshProUGUI? _attack;
    private GameObject? _grid;
    private TextMeshProUGUI? _hitpoint;
    private TextMeshProUGUI? _hitrate;
    private TextMeshProUGUI? _speed;
    private MonsterCollection Monsters => GetComponentInParent<MonsterCollection>();

    private new void Start()
    {
        if (!_lookupOnce &&
            _turnIcon == null &&
            ComponentFetch.TryFindObject("Action Count_Icon", out _turnIcon)) {
            _lookupOnce = true;
        }

        if (Monsters == null) {
            return;
        }

        _grid = transform
            .GetFirstNestedChildWithName("MonsterInfo/GridBG")?.gameObject;
        if (_grid == null) {
            return;
        }

        var story = _grid.transform.GetFirstChildWithName("Story");
        if (story == null) {
            return;
        }

        story.localPosition = story.localPosition with { y = story.localPosition.y - BasicHeightAdjust };

        _align = _grid.transform
            .GetFirstChildWithName("Stat/Debuff")?
            .GetFirstChildWithName("StatAlign")?.gameObject;
        if (_align == null) {
            return;
        }

        PrepareExtraEntries();
        AdjustStatPositions();
    }

    private void Update()
    {
        if (_align != null) {
            _align.transform
                .GetAllChildren()
                .Do(t => t.gameObject.SetActive(true));
        }
    }

    public void UpdateAttackNum(int attack)
    {
        if (_attack != null) {
            _attack.text = attack.ToString();
        }
    }

    public void UpdateHitpointNum(int hitpoint)
    {
        if (_hitpoint != null) {
            _hitpoint.text = $"<b>{EExLoc.Stat.Hitpoint}  {hitpoint}</b>";
        }
    }

    public void UpdateHitrateNum(int hitrate)
    {
        if (_hitrate != null) {
            _hitrate.text = hitrate.ToString();
        }
    }

    public void UpdateSpeedNum(int speed)
    {
        if (_speed != null) {
            _speed.text = speed.ToString();
        }
    }

    private void PrepareExtraEntries()
    {
        var attack = Instantiate(_align!.transform.GetChild(0).gameObject, _align.transform);
        attack.name = "Attack";
        DestroyLocalizer(attack);
        attack.GetComponentInChildren<TextMeshProUGUI>().text = EExLoc.Stat.Attack;
        _attack = attack.transform.GetFirstChildWithName("Num")?.GetComponent<TextMeshProUGUI>();

        var hitrate = Instantiate(_align.transform.GetChild(0).gameObject, _align.transform);
        hitrate.name = "Hitrate";
        DestroyLocalizer(hitrate);
        hitrate.GetComponentInChildren<TextMeshProUGUI>().text = EExLoc.Stat.Hitrate;
        _hitrate = hitrate.transform.GetFirstChildWithName("Num")?.GetComponent<TextMeshProUGUI>();

        hitrate.transform.SetAsFirstSibling();
        attack.transform.SetAsFirstSibling();

        var speedObj = DuplicateNumTextObj("Dot", -BasicHeightAdjust);
        speedObj.name = "Speed";
        speedObj.GetComponent<Image>().sprite = _turnIcon;
        DestroyLocalizer(speedObj);
        var speedText = speedObj.GetComponentInChildren<TextMeshProUGUI>();
        speedText.text = EExLoc.Stat.Speed;
        speedText.color = Color.white;

        var speedNumObj = DuplicateNumTextObj("DotNum", -BasicHeightAdjust);
        speedNumObj.name = "SpeedNum";
        _speed = speedNumObj.GetComponent<TextMeshProUGUI>();

        var hitpointNumObj = DuplicateNumTextObj("DotNum", BasicHeightAdjust * 3.8f);
        hitpointNumObj.name = "HpNum";
        _hitpoint = hitpointNumObj.GetComponent<TextMeshProUGUI>();
    }

    private GameObject DuplicateNumTextObj(string childName, float yOffset)
    {
        var stats = _grid!.transform.GetFirstChildWithName("Stat/Debuff");
        var @base = stats!.GetFirstChildWithName(childName)!.gameObject;
        var obj = Instantiate(@base, stats);
        MoveYAxis(obj.transform, yOffset);
        return obj;
    }

    private void AdjustStatPositions()
    {
        if (_align == null) {
            return;
        }

        var rect = _align.GetComponent<RectTransform>();
        rect.sizeDelta = rect.sizeDelta with { y = rect.sizeDelta.y + BasicHeightAdjust * 2 };
        MoveYAxis(rect, -BasicHeightAdjust);
    }

    private static void MoveYAxis(Transform transformIn, float offset)
    {
        var pos = transformIn.localPosition;
        transformIn.localPosition = pos with { y = pos.y + offset };
    }

    private static void DestroyLocalizer(GameObject go)
    {
        var localize = go.GetComponentInChildren<Localize>();
        if (localize != null) {
            Destroy(localize);
        }
    }
}