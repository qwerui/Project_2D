using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GachaController : MonoBehaviour
{
    private List<ItemData> AllItemData;
    private List<ItemData> ItemDropPool;

    int[] GachaRarity = new int[3]; 
    int[] GachaItemType = new int[3]; //0:무기, 1:방어구, 2:악세사리

    int maxRarityWeight;
    int maxItemTypeWeight;

    ItemType itemType;
    ItemData itemOutput;
    int rarity;

    // Start is called before the first frame update
    void Start()
    {
        AllItemData = ItemLoader.Instance.GetEquipItemList();
        InitPercentage();
    }

    void InitPercentage()
    {
        GachaRarity[0] = 890;
        GachaRarity[1] = 100;
        GachaRarity[2] = 10;
        GachaItemType[0] = 1000;
        GachaItemType[1] = 1000;
        GachaItemType[2] = 1000;

        maxRarityWeight = 1000;
        maxItemTypeWeight = 3000;

        ItemDropPool = null;
        itemOutput = null;
    }

    public ItemData ItemGacha(int gold, int red, int blue, int yellow)
    {
        InitPercentage();
        GachaRarity[1]+=(int)(gold*0.1f);
        GachaRarity[2]+=(int)(gold*0.01f);
        maxRarityWeight+=(int)(gold * 0.11f);
        GachaItemType[0]+=red;
        GachaItemType[1]+=blue;
        GachaItemType[2]+=yellow;
        maxItemTypeWeight+=red+blue+yellow;

        int RandomItemType = Random.Range(1,maxItemTypeWeight+1);
        int RandomRarity = Random.Range(1,maxRarityWeight+1);

        int itemTypeWeight = 0;
        int RarityWeight = 0;

        for(int i = 0; i<3; i++)
        {
            itemTypeWeight+=GachaItemType[i];
            if(RandomItemType <= itemTypeWeight)
            {
                if(i==0)
                    itemType = ItemType.Weapon;
                else if(i==1)
                    itemType = ItemType.Armor;
                else
                    itemType = ItemType.Accesory;
                break;
            }
        }
        for(int i = 0; i<3 ; i++)
        {
            RarityWeight+=GachaRarity[i];
            if(RandomRarity <= RarityWeight)
            {
                rarity = i;
                break;
            }
        }

        ItemDropPool = FilterItem(rarity, itemType, red, blue, yellow);
        itemOutput = ItemDropPool[Random.Range(0,ItemDropPool.Count)];

        return itemOutput;
    }
    List<ItemData> FilterItem(int _rarity, ItemType _itemType, int _red, int _blue, int _yellow)
    {
        List<ItemData> tempList = (from item in AllItemData where item.ItemType == _itemType && (item as EquipItemData).Rarity == _rarity select item).ToList();
        tempList = (from item in tempList where ((item as EquipItemData).Red <= _red && (item as EquipItemData).Blue <= _blue)&&(item as EquipItemData).Yellow <= _yellow select item).ToList();
        return tempList;
    }
}
