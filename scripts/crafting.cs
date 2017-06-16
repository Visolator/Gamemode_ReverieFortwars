//ReverieFortWars/crafting.cs

//TABLE OF CONTENTS

$RFW::T5Tools = "Sanguite crafting tools\tSinnite crafting tools";
$RFW::T4Tools = "Calcite crafting tools\tNithlite crafting tools" TAB $RFW::T5Tools;
$RFW::T3Tools = "Surtra crafting tools\tObsidian crafting tools" TAB $RFW::T4Tools;
$RFW::T2Tools = "Stone crafting tools" TAB $RFW::T3Tools;
$RFW::T1Tools = "Wooden crafting tools\tFaerie crafting tools" TAB $RFW::T2Tools;

function RFW_AddRecipe(%product, %materials, %requirements)
{
	$RFW::RecipeMats[%product] = %materials;
	$RFW::RecipeReqs[%product] = %requirements;
}

//resource conversions
RFW_AddRecipe("Cloth", "2 Plant matter", $RFW::T1Tools);
RFW_AddRecipe("Paper", "1 Plant matter\t1 Wood", $RFW::T1Tools);
RFW_AddRecipe("Leather", "3 Flesh", $RFW::T1Tools);
RFW_AddRecipe("Glass", "2 Sand", "Pyromancy wand");
RFW_AddRecipe("Stone", "2 Sand", "Pyromancy wand");
RFW_AddRecipe("Calcite", "5 Bone", "Ossimetallisynthesis staff");
RFW_AddRecipe("Sanguite", "10 Blood", "Sanguimetallisynthesis wand");

//tools
RFW_AddRecipe("Wooden crafting tools", "32 Wood");
RFW_AddRecipe("Faerie crafting tools", "32 Plant matter\t8 Faerie dust\t1 Wooden crafting tools");
RFW_AddRecipe("Stone crafting tools", "16 Stone\t1 Wooden crafting tools");
RFW_AddRecipe("Obsidian crafting tools", "32 Obsidian\t1 Stone crafting tools");
RFW_AddRecipe("Surtra crafting tools", "32 Surtra\t1 Stone crafting tools");
RFW_AddRecipe("Calcite crafting tools", "32 Calcite\t1 Obsidian crafting tools\n32 Calcite\t1 Surtra crafting tools");
RFW_AddRecipe("Nithlite crafting tools", "32 Nithlite\t1 Obsidian crafting tools\n32 Nithlite\t1 Surtra crafting tools");
RFW_AddRecipe("Sanguite crafting tools", "32 Sanguite\t1 Calcite crafting tools");
RFW_AddRecipe("Sinnite crafting tools", "32 Sinnite\t1 Nithlite crafting tools");

RFW_AddRecipe("Faerie wand", "8 Wood\t8 Plant matter\t8 Faerie dust", "Faerie crafting tools");
RFW_AddRecipe("Pyromancy wand", "16 Wood\t8 Plant matter\t8 Faerie dust", "Faerie crafting tools");
RFW_AddRecipe("Alchemy vials", "16 Glass", "Pyromancy wand");
RFW_AddRecipe("Photometallisynthesis lens", "32 Glass\t32 Plant matter\t8 Faerie dust", "Pyromancy wand");
RFW_AddRecipe("Ossimetallisynthesis staff", "32 Bone\t32 Calcite\t8 Faerie dust", "Faerie crafting tools");
RFW_AddRecipe("Sanguimetallisynthesis wand", "32 Blood\t32 Sanguite\t8 Faerie dust", "Faerie crafting tools");

//weapons
RFW_AddRecipe("Snowball", "2 Snow");

RFW_AddRecipe("Wooden sword", "32 Wood", $RFW::T1Tools);
RFW_AddRecipe("Wooden bow", "64 Wood\t8 Plant matter", $RFW::T1Tools);
RFW_AddRecipe("Wooden quiver", "24 Wood", $RFW::T1Tools);
RFW_AddRecipe("Wooden armor", "128 Wood", $RFW::T1Tools);

RFW_AddRecipe("Bone sword", "32 Bone", $RFW::T1Tools);
RFW_AddRecipe("Bone bow", "64 Bone\t8 Plant matter", $RFW::T1Tools);
RFW_AddRecipe("Bone quiver", "24 Bone", $RFW::T1Tools);
RFW_AddRecipe("Bone armor", "128 Bone", $RFW::T1Tools);

RFW_AddRecipe("Faerie bow", "16 Plant matter\t8 Faerie dust\t1 Wooden bow", "Faerie crafting tools");
RFW_AddRecipe("Faerie armor", "128 Plant matter\t8 Faerie dust", "Faerie crafting tools");

RFW_AddRecipe("Stone sword", "8 Wood\t32 Stone", $RFW::T2Tools);
RFW_AddRecipe("Stone quiver", "16 Wood\t16 Stone", $RFW::T2Tools);
RFW_AddRecipe("Stone pickaxe", "8 Wood\t32 Stone", $RFW::T2Tools);
RFW_AddRecipe("Stone armor", "128 Stone", $RFW::T2Tools);

RFW_AddRecipe("Obsidian sword", "24 Wood\t8 Obsidian", $RFW::T3Tools);
RFW_AddRecipe("Obsidian dagger", "24 Obsidian", $RFW::T3Tools);
RFW_AddRecipe("Obsidian quiver", "16 Wood\t16 Obsidian", $RFW::T3Tools);

RFW_AddRecipe("Surtra sword", "8 Wood\t32 Surtra", $RFW::T3Tools);
RFW_AddRecipe("Surtra dagger", "4 Wood\t24 Surtra", $RFW::T3Tools);
RFW_AddRecipe("Surtra pickaxe", "8 Wood\t48 Surtra", $RFW::T3Tools);
RFW_AddRecipe("Surtra quiver", "16 Wood\t16 Surtra", $RFW::T3Tools);
RFW_AddRecipe("Surtra armor", "128 Surtra", $RFW::T3Tools);

RFW_AddRecipe("Calcite sword", "8 Wood\t32 Calcite", $RFW::T4Tools);
RFW_AddRecipe("Calcite dagger", "4 Wood\t24 Calcite", $RFW::T4Tools);
RFW_AddRecipe("Calcite pickaxe", "8 Wood\t48 Calcite", $RFW::T4Tools);
RFW_AddRecipe("Calcite quiver", "16 Wood\t16 Calcite", $RFW::T4Tools);
RFW_AddRecipe("Calcite armor", "128 Calcite", $RFW::T4Tools);

RFW_AddRecipe("Nithlite sword", "8 Wood\t32 Nithlite", $RFW::T4Tools);
RFW_AddRecipe("Nithlite dagger", "4 Wood\t24 Nithlite", $RFW::T4Tools);
RFW_AddRecipe("Nithlite pickaxe", "8 Wood\t48 Nithlite", $RFW::T4Tools);
RFW_AddRecipe("Nithlite quiver", "16 Wood\t16 Nithlite", $RFW::T4Tools);
RFW_AddRecipe("Nithlite armor", "128 Nithlite", $RFW::T4Tools);

RFW_AddRecipe("Sanguite sword", "8 Wood\t32 Sanguite", $RFW::T5Tools);
RFW_AddRecipe("Sanguite dagger", "4 Wood\t24 Sanguite", $RFW::T5Tools);
RFW_AddRecipe("Sanguite pickaxe", "8 Wood\t48 Sanguite", $RFW::T5Tools);
RFW_AddRecipe("Sanguite quiver", "16 Wood\t16 Sanguite", $RFW::T5Tools);
RFW_AddRecipe("Sanguite armor", "128 Sanguite", $RFW::T5Tools);

RFW_AddRecipe("Sinnite sword", "8 Wood\t32 Sinnite", $RFW::T5Tools);
RFW_AddRecipe("Sinnite dagger", "4 Wood\t24 Sinnite", $RFW::T5Tools);
RFW_AddRecipe("Sinnite pickaxe", "8 Wood\t48 Sinnite", $RFW::T5Tools);
RFW_AddRecipe("Sinnite quiver", "16 Wood\t16 Sinnite", $RFW::T5Tools);
RFW_AddRecipe("Sinnite armor", "128 Sinnite", $RFW::T5Tools);

RFW_AddRecipe("Crystal sword", "8 Wood\t32 Crystal", $RFW::T5Tools);
RFW_AddRecipe("Crystal dagger", "4 Wood\t24 Crystal", $RFW::T5Tools);
RFW_AddRecipe("Crystal pickaxe", "8 Wood\t48 Crystal", $RFW::T5Tools);
RFW_AddRecipe("Crystal quiver", "16 Wood\t16 Crystal", $RFW::T5Tools);
RFW_AddRecipe("Crystal armor", "128 Crystal", $RFW::T5Tools);

//clothes
RFW_AddRecipe("Cloth shirt", "24 cloth", $RFW::T1Tools);
RFW_AddRecipe("Cloth pants", "24 cloth", $RFW::T1Tools);
RFW_AddRecipe("Cloth hat", "24 cloth", $RFW::T1Tools);
RFW_AddRecipe("Cloth cape", "16 cloth", $RFW::T1Tools);
RFW_AddRecipe("Cloth robe", "64 cloth", $RFW::T1Tools);
RFW_AddRecipe("Leather boots", "16 Leather", $RFW::T1Tools);
RFW_AddRecipe("Leather gloves", "16 Leather", $RFW::T1Tools);

//medicines and potions
RFW_AddRecipe("Cloth bandage", "2 cloth\t1 plant matter");
RFW_AddRecipe("Pishelva", "5 plant matter\t1 dirt\t1 faerie dust", "Alchemy vials");
RFW_AddRecipe("Hoborchakin", "5 plant matter\t1 wood\t1 faerie dust", "Alchemy vials");
RFW_AddRecipe("Vorpal potion", "1 sanguite\t1 calcite\t10 plant matter", "Alchemy vials");
RFW_AddRecipe("Fleshweaver's potion", "3 plant matter\t3 flesh\t3 blood\t1 bone\t1 faerie dust", "Alchemy vials");
RFW_AddRecipe("Crimson potion", "10 flesh\t2 blood\t1 faerie dust", "Alchemy vials");