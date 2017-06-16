//ReverieFortWars/accounts.cs

//TABLE OF CONTENTS
// #1. functionality
//   #1.1 RFW_Account functions
//   #1.2 inventory functions
//   #1.3 interfacing
//   #1.4 hasRequiredItems
//   #1.5 getBestArrow
// #2. package

// #1.
// #1.1

if(!isObject("RFW_Accounts"))
	new SimGroup("RFW_Accounts");

function RFW_NewAccount()
{
	%acc = new ScriptObject()
	{
		class = "RFW_Account";
		vMaxExp = getLevelStat(1, "exp");
		vExp = 0;
		vHealth = getLevelStat(1, "hp");
		vLevel = 1;
		vSkillPoints = 0;
		vKarma = 0;
		vMaxItems = 50;
		vMaxEquips = 5;
		vEquip0 = "Fists\t0\t";
		vUseCPQ = true;
		vVer = 0;
	};
	%tribe = getWord($RFW::ActiveTribes, getRandom(0, getWordCount($RFW::ActiveTribes) - 1));
	%acc.set("Tribe", %tribe);
	RFW_Accounts.add(%acc);
	return %acc;
}

function RFW_Account::Read(%acc, %var)
{
	return %acc.v[%var];
}

function RFW_Account::Set(%acc, %var, %value)
{
	if(%acc.readOnly)
	{
		return false;
	}
	else
	{
		%acc.v[%var] = %value;
		return true;
	}
}

function RFW_Account::Inc(%acc, %var, %amt)
{
	if(%acc.readOnly)
	{
		return false;
	}
	else
	{
		%acc.v[%var]+= %value;
		return true;
	}
}

// #1.2

//returns a bool: whether this %acc has an instance of %item
//%item should be specified as the item's name
function RFW_Account::hasItem(%acc, %item)
{
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		if(getItemType(%acc.read("Item" @ %i)) $= %item)
		{
			return true;
		}
	}
	return false;
}

//returns a bool: whether this %acc has at least some amount of an item
//%item must be an item string (use the quantity field)
function RFW_Account::hasItemQuantity(%acc, %item)
{
	%type = getItemType(%item);
	%amt = getItemQuantity(%item);
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		%it = %acc.read("Item" @ %i);
		if(getItemType(%it) $= %type)
		{
			if(!$RFW::ItemStackable[%type])
			{
				%count++;
			}
			else
			{
				return getItemQuantity(%it) >= %amt;
			}
		}
	}
	return %count >= %amt;
}

//returns an int: the number of the specified item possessed by the account
//%item must be an item type
function RFW_Account::getItemQuantity(%acc, %item)
{
	%type = getItemType(%item);
	%max = %acc.read("MaxItems");
	%count = 0;
	for(%i = 0; %i < %max; %i++)
	{
		%it = %acc.read("Item" @ %i);
		if(getItemType(%it) $= %type)
		{
			if(!$RFW::ItemStackable[%type])
			{
				%count++;
			}
			else
			{
				return getItemQuantity(%it);
			}
		}
	}
	return %count;
}

//returns a string corresponding to the item, or -1 if the item was not found
//%item should be specified by name
function RFW_Account::getItem(%acc, %item)
{
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		%it = %acc.read("Item" @ %i);
		if(getItemType(%it) $= %item)
		{
			return %it;
		}
	}
	return -1;
}

//returns a NL-delimited string, where each line is an item possessed by the player
//%item should be specified by name
function RFW_Account::getItemList(%acc)
{
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		%it = getItemType(%acc.read("Item" @ %i));
		if(%it !$= "")
		{
			%out = %out NL %it;
		}
	}
	return removeRecord(%out, 0);
}

//returns an int: the slot the item is in, or -1 if the item was not found
//%item should be specified as the item's name
function RFW_Account::getItemSlot(%acc, %item)
{
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		if(getItemType(%acc.read("Item" @ %i)) $= %item)
		{
			return %i;
		}
	}
	return -1;
}

//adds %item to %acc's inventory
//%item must be an item string (see items.cs documentation)
//returns a bool: whether adding the item to inventory was successful or not
function RFW_Account::addItem(%acc, %item, %display)
{
	%type = getItemType(%item);
	%stackable = $RFW::ItemStackable[%type];
	%max = %acc.read("MaxItems");
	%freeSpace = -1;
	for(%i = 0; %i < %max; %i++)
	{
		%it = %acc.read("Item" @ %i);
		if(%stackable && getItemType(%it) $= %type)
		{
			%amt = getItemQuantity(%item);
			%item = incItemQuantity(%it, %amt);
			if(%display)
			{
				%acc.client.centerPrintQueue("\c2Got " @ %amt @ "x " @ %type @ " (" @ getItemQuantity(%item) @ "x total)", 5);
			}
			return %acc.set("Item" @ %i, %item);
		}
		else if(%freeSpace == -1 && !strLen(%it))
		{
			if(!%stackable)
			{
				if(%display)
				{
					%acc.client.centerPrintQueue("\c2Got " @ %type, 5);
					%maxEquips = %acc.read("MaxEquips");
					if($RFW::ItemEquipable[%type])
					{
						for(%i = 0; %i < %maxEquips; %i++)
						{
							if(%acc.read("Equip" @ %i) $= "")
							{
								schedule(0, 0, serverCmdFromPack, %acc.client, %type);
							}
						}
					}
					else if($RFW::ItemWearable[%type])
					{
						if(%acc.read($RFW::ItemSlot[%type]) $= "")
						{
							schedule(0, 0, serverCmdEquip, %acc.client, %type);
						}
					}
				}
				return %acc.set("Item" @ %i, %item);
			}
			%freeSpace = %i;
		}
	}
	if(%freeSpace != -1)
	{
		if(%stackable)
		{
			%amt = getItemQuantity(%item);
			if(%display)
			{
				%acc.client.centerPrintQueue("\c2Got " @ %amt @ "x " @ %type @ " (" @ %amt @ "x total)", 5);
			}
		}
		else if(%display)
		{
			%acc.client.centerPrintQueue("\c2Got " @ %type, 5);
			if($RFW::ItemEquipable[%type])
			{
				//!!! TODO: make it so this only happens if there's room
				schedule(0, 0, serverCmdFromPack, %acc.client, %type);
			}
			else if($RFW::ItemWearable[%type])
			{
				//!!! TODO: make it so this only happens if there's room
				schedule(0, 0, serverCmdEquip, %acc.client, %type);
			}
		}
		return %acc.set("Item" @ %freeSpace, %item);
	}
	return false;
}

//adds %item to %acc's inventory
//%item must be an item string (see items.cs documentation)
//%display is a bool which sets whether any information is sent to the user
//returns a bool: whether removing the item from inventory was successful or not
function RFW_Account::removeItem(%acc, %item, %display)
{
	%stackable = $RFW::ItemStackable[getItemType(%item)];
	%max = %acc.read("MaxItems");
	for(%i = 0; %i < %max; %i++)
	{
		%it = %acc.read("Item" @ %i);
		if(getItemType(%it) $= getItemType(%item))
		{
			if(%stackable)
			{
				%amt = -1 * getItemQuantity(%item);
				%item = incItemQuantity(%it, %amt);
				if(getItemQuantity(%item) < 0)
				{
					return false;
				}
				if(%display)
				{
					%acc.client.centerPrintQueue("<color:FF0000>Lost " @ %amt @ "x " @ getItemType(%item), 5);
				}
				if(getItemQuantity(%item) == 0)
				{
					%result = %acc.set("Item" @ %i, "");
					%acc.resortInventory();
					return %result;
				}
				else
				{
					return %acc.set("Item" @ %i, %item);
				}
			}
			else
			{
				if(%display)
				{
					%acc.client.centerPrintQueue("<color:FF0000>Lost " @ getItemType(%item), 5);
				}
				%result = %acc.set("Item" @ %i, "");
				%acc.resortInventory();
				return %result;
			}
		}
	}
	return false;
}

function RFW_Account::resortInventory(%acc)
{
	%maxItems = %acc.read("MaxItems");
	%drop = 0;
	for(%peek = 0; %peek < %maxItems; %peek++)
	{
		%item = %acc.read("Item" @ %peek);
		if(getItemType(%item) !$= "")
		{
			if(%peek != %drop)
			{
				%acc.set("Item" @ %peek, "");
				%acc.set("Item" @ %drop, %item);
			}
			%drop++;
		}
	}
}

// #1.4
function RFW_Account::hasRequiredItems(%acc, %mats, %tools, %mult)
{
	if(%tools !$= "")
	{
		%hasTools = false;
		%fieldCount = getFieldCount(%tools);
		for(%i = 0; %i < %fieldCount; %i++)
		{
			%tool = getField(%tools, %i);
			if(%acc.hasItem(%tool))
			{
				%hasTools = true;
				break;
			}
		}
		if(!%hasTools)
		{
			return false;
		}
	}
	if(%mult !$= "")
	{
		%fieldCount = getFieldCount(%mats);
		for(%i = 0; %i < %fieldCount; %i++)
		{
			%field = getField(%mats, %i);
			%mats = setField(%mats, %i, setWord(%field, 0, firstWord(%field) * %mult));
		}
	}
	%fieldCount = getFieldCount(%mats);
	for(%i = 0; %i < %fieldCount; %i++)
	{
		%item = getField(%mats, %i);
		%item = restWords(%item) TAB firstWord(%item);
		if(!%acc.hasItemQuantity(%item))
		{
			return false;
		}
	}
	return true;
}

function RFW_Account::GetEquipmentList(%acc)
{
	%list = "";
	for(%i = 0; %i < %acc.read("MaxEquips"); %i++)
	{
		%eq = %acc.read("Equip" @ %i);
		if(%eq !$= "")
		{
			%list = %list NL getItemType(%eq);
		}
	}
	if((%eq = %acc.read("Armor")) !$= "")
	{
		%list = %list NL getItemType(%eq);
	}
	if((%eq = %acc.read("Back")) !$= "")
	{
		%list = %list NL getItemType(%eq);
	}
	if((%eq = %acc.read("Hat")) !$= "")
	{
		%list = %list NL getItemType(%eq);
	}
	if((%eq = %acc.read("Shirt")) !$= "")
	{
		%list = %list NL getItemType(%eq);
	}
	if((%eq = %acc.read("Pants")) !$= "")
	{
		%list = %list NL getItemType(%eq);
	}
	if(getRecordCount(%list) > 0)
	{
		return removeRecord(%list, 0);
	}
	else
	{
		return "";
	}
}

function RFW_Account::GetOptions(%acc)
{
	return "nothing\nyet";
}

function RFW_Account::AddFavor(%acc, %amt, %tribe)
{
	%accTribe = %acc.read("Tribe");
	if(%accTribe !$= %tribe && %accTribe !$= "")
	{
		return false;
	}
	%acc.inc("Favor" @ %tribe, %amt);
}

function RFW_FixAccounts()
{
	//RFW_Accounts
}

if(isPackage("RFW_Accounts"))
	deactivatePackage("RFW_Accounts");

// #2.
package RFW_Accounts
{
	function GameConnection::autoAdminCheck(%client)
	{
		%p = parent::autoAdminCheck(%client);
		if(!$RFW::IgnoreID[%client.getBLID()])
		{
			if($RFW::IgnoreID[%client.getBLID()])
			{
				%client.loadBetaAccount();
				return %p;
			}

			%file = "config/server/RFW/" @ %client.bl_id @ ".cs";
			if(isFile(%file))
			{
				exec(%file);
				%acc = NewlyLoaded.getID();
				if(isObject(%acc))
				{
					%acc.setName("");
					schedule(0, messageClient, %client, '', "\c2RFW account loaded successfully.");
				}
				else
				{
					error("Account loading failed (" @ %file @ ")");
					%acc = RFW_NewAccount();
				}
			}
			else
			{
				%acc = RFW_NewAccount();
				schedule(0, messageClient, %client, '', "\c2RFW account created successfully.");
			}

			%acc.set("Name", %client.getPlayerName());
			RFW_Accounts.add(%acc);
			%client.account = %acc;
			%acc.client = %client;
			%client.brickgroup.tribe = %acc.read("Tribe");
			if(%acc.vMaxExp $= "")
				%acc.vMaxExp = getLevelStat(1, "exp");
			if(%acc.vExp $= "")
				%acc.vExp = 0;
			if(%acc.vHealth $= "")
				%acc.vHealth = getLevelStat(1, "hp");
			if(%acc.vSkillPoints $= "")
				%acc.vSkillPoints = 0;
			if(%acc.vLevel $= "")
				%acc.vLevel = 1;
		}

		return %p;
	}

	function GameConnection::onDrop(%client)
	{
		%acc = %client.account;
		if(isObject(%acc))
		{
			%acc.setName("NewlyLoaded");
			%acc.save("config/server/RFW/" @ %client.bl_id @ ".cs");
			%acc.delete();
		}
		return parent::onDrop(%client);
	}
};
activatePackage(RFW_Accounts);