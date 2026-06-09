using System.Collections;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// API surface for interacting with player inventory and item management.
    /// </summary>
    public sealed class BBPItems
    {
        /// <summary>
        /// Underlying item manager object from the game.
        /// </summary>
        public object? Manager => BBP.Player.ItemManager;

        /// <summary>
        /// Currently selected inventory slot index.
        /// </summary>
        public int SelectedSlot
        {
            get => ReflectionUtil.GetField<int>(Manager, "selectedItem");
            set
            {
                ReflectionUtil.SetField(Manager, "selectedItem", value);
                UpdateSelect();
            }
        }

        /// <summary>
        /// Maximum slot index available.
        /// </summary>
        public int MaxSlot => ReflectionUtil.GetField<int>(Manager, "maxItem");
        /// <summary>
        /// Total number of inventory slots (MaxSlot + 1).
        /// </summary>
        public int SlotCount => MaxSlot + 1;

        /// <summary>
        /// Default configured inventory size for the player.
        /// </summary>
        public int DefaultInventorySize
        {
            get => ReflectionUtil.GetField<int>(Manager, "defaultInventorySize");
            set
            {
                ReflectionUtil.SetField(Manager, "defaultInventorySize", value);
                UpdateTargetInventorySize();
            }
        }

        /// <summary>
        /// Total number of items currently in the player's inventory.
        /// </summary>
        public int TotalItems =>
            ReflectionUtil.GetProperty<int>(Manager, "TotalItemsInInventory");

        /// <summary>
        /// Highest slot index that currently contains an item.
        /// </summary>
        public int MaxSlotWithItem =>
            ReflectionUtil.GetProperty<int>(Manager, "MaxSlotWithItem");

        /// <summary>
        /// Represents the 'nothing' placeholder object used by the item manager.
        /// </summary>
        public object? Nothing =>
            ReflectionUtil.GetField<object>(Manager, "nothing");

        /// <summary>
        /// The raw underlying list of inventory item objects.
        /// </summary>
        public IList? RawItems =>
            ReflectionUtil.GetField<IList>(Manager, "items");

        /// <summary>
        /// Finds an item object from the game's item database corresponding to the enum id.
        /// </summary>
        /// <param name="id">The item enum id to look up.</param>
        /// <returns>The matching item object or null.</returns>
        public object? GetItemObject(BBPItemId id)
        {
            object? wantedType = ReflectionUtil.GetEnumValue("Items", id.ToString());
            if (wantedType == null)
                return null;

            IList? itemObjects = GetItemDatabase();
            if (itemObjects == null)
                return null;

            foreach (object? item in itemObjects)
            {
                if (item == null)
                    continue;

                object? itemType = ReflectionUtil.GetField<object>(item, "itemType");

                if (itemType != null && itemType.Equals(wantedType))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Retrieves an item object by its index in the game's item database.
        /// </summary>
        /// <param name="index">Database index to retrieve.</param>
        /// <returns>The item object or null if index is invalid.</returns>
        public object? GetItemObjectByDatabaseIndex(int index)
        {
            IList? itemObjects = GetItemDatabase();

            if (itemObjects == null || index < 0 || index >= itemObjects.Count)
                return null;

            return itemObjects[index];
        }

        private static IList? GetItemDatabase()
        {
            object? pfm = ReflectionUtil.GetSingletonInstance("PlayerFileManager");
            return ReflectionUtil.GetField<IList>(pfm, "itemObjects");
        }

        /// <summary>
        /// Gets the item object from the player's inventory slot.
        /// </summary>
        /// <param name="slot">Zero-based inventory slot index.</param>
        /// <returns>The item object or null if the slot is empty or invalid.</returns>
        public object? GetSlot(int slot)
        {
            IList? items = RawItems;

            if (items == null || slot < 0 || slot >= items.Count)
                return null;

            return items[slot];
        }

        /// <summary>
        /// Gets the display name for the item contained in the given slot.
        /// </summary>
        /// <param name="slot">Inventory slot index.</param>
        /// <returns>The item name or "NULL" if empty.</returns>
        public string GetSlotName(int slot)
        {
            return GetItemName(GetSlot(slot));
        }

        /// <summary>
        /// Returns a human-readable name for the provided item object.
        /// </summary>
        /// <param name="item">The item object to inspect; may be null.</param>
        /// <returns>A display name string for the item.</returns>
        public string GetItemName(object? item)
        {
            if (item == null)
                return "NULL";

            string? nameKey = ReflectionUtil.GetField<string>(item, "nameKey");
            if (!string.IsNullOrEmpty(nameKey))
                return nameKey!;

            object? itemType = ReflectionUtil.GetField<object>(item, "itemType");
            return itemType?.ToString() ?? item.ToString();
        }

        /// <summary>
        /// Attempts to add an item of the specified id to the player's inventory.
        /// </summary>
        /// <param name="id">Item enum id to add.</param>
        /// <returns>True if the item was added; otherwise false.</returns>
        public bool Add(BBPItemId id)
        {
            object? item = GetItemObject(id);
            if (item == null)
                return false;

            ReflectionUtil.Call(Manager, "AddItem", item);
            return true;
        }

        /// <summary>
        /// Sets a specific inventory slot to contain the item with the given id.
        /// </summary>
        /// <param name="slot">Slot index to modify.</param>
        /// <param name="id">Item id to place into the slot.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool SetSlot(int slot, BBPItemId id)
        {
            object? item = GetItemObject(id);
            if (item == null)
                return false;

            ReflectionUtil.Call(Manager, "SetItem", item, slot);
            return true;
        }

        /// <summary>
        /// Removes the item from the specified inventory slot.
        /// </summary>
        /// <param name="slot">Slot index to clear.</param>
        public void RemoveSlot(int slot)
        {
            ReflectionUtil.Call(Manager, "RemoveItem", slot);
        }

        /// <summary>
        /// Removes the first instance of the specified item id from the inventory.
        /// </summary>
        /// <param name="id">Item id to remove.</param>
        public void Remove(BBPItemId id)
        {
            object? enumValue = ReflectionUtil.GetEnumValue("Items", id.ToString());

            if (enumValue != null)
                ReflectionUtil.Call(Manager, "Remove", enumValue);
        }

        /// <summary>
        /// Clears all items from the player's inventory.
        /// </summary>
        public void Clear()
        {
            ReflectionUtil.Call(Manager, "ClearItems");
        }

        /// <summary>
        /// Returns whether the player has at least one instance of the specified item.
        /// </summary>
        /// <param name="id">Item id to check.</param>
        /// <returns>True if the player has the item; otherwise false.</returns>
        public bool Has(BBPItemId id)
        {
            object? enumValue = ReflectionUtil.GetEnumValue("Items", id.ToString());

            if (enumValue == null)
                return false;

            return ReflectionUtil.Call<bool>(Manager, "Has", enumValue);
        }

        /// <summary>
        /// Returns true if the player has any usable item available.
        /// </summary>
        public bool HasAnyUsableItem()
        {
            return ReflectionUtil.Call<bool>(Manager, "HasItem");
        }

        /// <summary>
        /// Returns whether the player's inventory is currently full.
        /// </summary>
        public bool InventoryFull()
        {
            return ReflectionUtil.Call<bool>(Manager, "InventoryFull");
        }

        /// <summary>
        /// Uses the currently selected item.
        /// </summary>
        public void UseSelected()
        {
            ReflectionUtil.Call(Manager, "UseItem");
        }

        /// <summary>
        /// Removes a random item from the player's inventory.
        /// </summary>
        public void RemoveRandom()
        {
            ReflectionUtil.Call(Manager, "RemoveRandomItem");
        }

        /// <summary>
        /// Locks or unlocks the specified inventory slot.
        /// </summary>
        /// <param name="slot">Slot index to lock/unlock.</param>
        /// <param name="locked">True to lock; false to unlock.</param>
        public void LockSlot(int slot, bool locked)
        {
            ReflectionUtil.Call(Manager, "LockSlot", slot, locked);
        }

        /// <summary>
        /// Enables or disables the item manager.
        /// </summary>
        /// <param name="disabled">True to disable; false to enable.</param>
        public void Disable(bool disabled)
        {
            ReflectionUtil.Call(Manager, "Disable", disabled);
        }

        /// <summary>
        /// Reduces the target inventory size by one.
        /// </summary>
        public void ReduceTargetInventorySize()
        {
            ReflectionUtil.Call(Manager, "ReduceTargetInventorySize");
        }

        /// <summary>
        /// Resets the maximum item slot to the default value.
        /// </summary>
        public void ResetMaxItem()
        {
            ReflectionUtil.Call(Manager, "ResetMaxItem");
        }

        /// <summary>
        /// Triggers the internal item update logic.
        /// </summary>
        public void UpdateItems()
        {
            ReflectionUtil.Call(Manager, "UpdateItems");
        }

        /// <summary>
        /// Updates selection visuals and internal state after SelectedSlot changes.
        /// </summary>
        public void UpdateSelect()
        {
            ReflectionUtil.Call(Manager, "UpdateSelect");
        }

        /// <summary>
        /// Applies the configured target inventory size to the manager.
        /// </summary>
        public void UpdateTargetInventorySize()
        {
            ReflectionUtil.Call(Manager, "UpdateTargetInventorySize");
        }
    }
}