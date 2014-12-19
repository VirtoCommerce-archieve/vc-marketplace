# variables
Param( 
	#$db = "VirtoCommerce",
    $db = "VirtoMarketPlace",
    $serverinstance="tcp:kiy0km2fxn.database.windows.net,1433",
    #$serverinstance="(local)",
    $user = "Virto",
    $pwd = "Med1achase",
    $useWidowsLogin = $false,
    $datafolder
)

if (!$datafolder) {
	$datafolder = Split-Path -Parent $MyInvocation.MyCommand.Path
}


echo $datafolder

if($useWidowsLogin)
{
invoke-sqlcmd -inputfile "$($datafolder)\dbo_CatalogBase.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Catalog.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_CatalogLanguage.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Store.sql" -serverinstance $serverinstance -database $db

invoke-sqlcmd -inputfile "$($datafolder)\dbo_Property.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertyValue.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertySet.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertySetProperty.sql" -serverinstance $serverinstance -database $db

invoke-sqlcmd -inputfile "$($datafolder)\dbo_CategoryBase.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Category.sql" -serverinstance $serverinstance -database $db

invoke-sqlcmd -inputfile "$($datafolder)\dbo_Item.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemPropertyValue.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemRelation.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemAsset.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_EditorialReview.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_CategoryItemRelation.sql" -serverinstance $serverinstance -database $db

invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentPlace.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentItem.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentItemProperty.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentPublishingGroup.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PublishingGroupContentItem.sql" -serverinstance $serverinstance -database $db
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PublishingGroupContentPlace.sql" -serverinstance $serverinstance -database $db
}
else
{

invoke-sqlcmd -inputfile "$($datafolder)\dbo_CatalogBase.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Catalog.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_CatalogLanguage.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Store.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd

invoke-sqlcmd -inputfile "$($datafolder)\dbo_Property.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertyValue.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertySet.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PropertySetProperty.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd

invoke-sqlcmd -inputfile "$($datafolder)\dbo_CategoryBase.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_Category.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd

invoke-sqlcmd -inputfile "$($datafolder)\dbo_Item.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemPropertyValue.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemRelation.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_ItemAsset.sql" -serverinstance $serverinstance -database $db  -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_EditorialReview.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_CategoryItemRelation.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd

invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentPlace.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentItem.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentItemProperty.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_DynamicContentPublishingGroup.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PublishingGroupContentItem.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
invoke-sqlcmd -inputfile "$($datafolder)\dbo_PublishingGroupContentPlace.sql" -serverinstance $serverinstance -database $db -Username $user -Password $pwd
}
