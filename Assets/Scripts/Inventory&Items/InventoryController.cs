using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    public int Capacity { get; private set; }
    [SerializeField] private Item[] _items; //실제 인벤토리 객체
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private DataController data;

    private readonly HashSet<int> _indexSetForUpdate = new HashSet<int>();

    private void Awake() {
        Capacity = 20;
        _items = new Item[Capacity];
        _inventoryUI.SetInventoryReference(this);
        _inventoryUI.InitSlots();
    }
    private void Start() {
        if(DataDirector.Instance.isLoadedGame)
        {
            for(int i=0;i<data.GetLoadedItemCount();i++)
            {
                _items[i] = data.LoadItem(i);
            }
            UpdateAllSlot();
        }
    }
    
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }

    // 해당 슬롯의 아이템 정보 리턴
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (_items[index] == null) return null;

        return _items[index].Data;
    }

    // 해당 슬롯의 아이템 이름 리턴
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return "";
        if (_items[index] == null) return "";

        return _items[index].Data.Name;
    }


    // 앞에서부터 비어있는 슬롯 인덱스 탐색
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
            if (_items[i] == null)
                return i;
        return -1;
    }
    public void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _items[index];
        
        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            if(item.Data.IconSprite != null)
            {
                
                _inventoryUI.SetItemIcon(index, item.Data.IconSprite); // 아이콘 등록
            }
            
            // 1-1. 셀 수 있는 아이템
            if (item is SubItem ci)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. 수량 텍스트 표시
                else
                {   
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
                // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
            else
            {
                _inventoryUI.HideItemAmountText(index);
            }
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
            _inventoryUI.RemoveItem(index);
            _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
        }
    }

    // 해당하는 인덱스의 슬롯들의 상태 및 UI 갱신
    private void UpdateSlot(params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i);
        }
    }

    //모든 슬롯들의 상태를 UI에 갱신
    private void UpdateAllSlot()
    {
        for (int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i);
        }
    }

    // 아이템 획득
    public int Add(ItemData itemData, GameObject player, int amount = 1)
    {
        int index;

        // 1. 수량이 있는 아이템
        if (itemData is SubItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        SubItem ci = _items[index] as SubItem;
                        ci.AddAmount(amount);
                        amount = 0;
                        UpdateSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        break;
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        // 새로운 아이템 생성
                        SubItem ci = ciData.CreateItem() as SubItem;
                        ci.SetPlayer(player);
                        ci.SetAmount(amount);
                        // 슬롯에 추가
                        _items[index] = ci;
                        amount=0;
                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. 수량이 없는 아이템
        else
        {
            // 2-1. 1개만 넣는 경우, 간단히 수행
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    // 아이템을 생성하여 슬롯에 추가
                    _items[index] = itemData.CreateItem();
                    _items[index].SetPlayer(player);
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            // 2-2. 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
            index = -1;
            for (; amount > 0; amount--)
            {
                // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                index = FindEmptySlotIndex(index + 1);

                // 다 넣지 못한 경우 루프 종료
                if (index == -1)
                {
                    break;
                }

                // 아이템을 생성하여 슬롯에 추가
                _items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }

            return amount;
    }
    // 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 
    private int FindCountableItemSlotIndex(SubItemData target, int startIndex = 0)
    {
       for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current.Data == target && current is SubItem ci)
            {
                    return i;
            }
        }
        return -1;
    }
    //해당 슬롯의 아이템 제거
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        _items[index] = null;
        _inventoryUI.RemoveItem(index);
    }

    public void TrimAll()
    {
            // 가장 빠른 배열 빈공간 채우기 알고리즘

            // i 커서와 j 커서
            // i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
            // j 커서 : i 커서 위치에서부터 뒤로 이동하며 기존재 아이템을 찾는 커서

            // i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 탐색
            // j커서가 아이템을 찾으면 아이템을 옮기고, i 커서는 i+1 위치로 이동
            // j커서가 Capacity에 도달하면 루프 즉시 종료

        _indexSetForUpdate.Clear();

        int i = -1;
        while (_items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && _items[j] == null);

            if (j == Capacity)
                break;

            _indexSetForUpdate.Add(i);
            _indexSetForUpdate.Add(j);

            _items[i] = _items[j];
            _items[j] = null;
            i++;
        }

        foreach (var index in _indexSetForUpdate)
        {
            UpdateSlot(index);
        }
    }

    public Item ReturnItem(int index)
    {
            return _items[index];
    }
    public void SetItem(int index, Item item)
    {
        _items[index] = item;
    }
    //장비 해제 아이템 추가
    public bool AddUnequipItem(Item item)
    {
        int index = -1;
        index = FindEmptySlotIndex(index + 1);
        if(index == -1)
            return false;
        _items[index] = item;
        UpdateSlot(index);
        return true;
    }
    public int GetExistItemCount()
    {
        for(int i=0;i<20;i++)
        {
            if(_items[i] == null)
                return i;
        }
        return 20;
    }
}
