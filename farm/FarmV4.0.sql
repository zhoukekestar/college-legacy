if object_id('farm_table') is null	 --鸡场
Create Table farm_table
(
FarmNum			int,					--编码
FarmName		nvarchar(200),			--名称
IsUse			bit default 0,			--是否在用 0 false 非0 true	
DisplayOrder	int default 1,			--显示顺序
Primary Key(FarmNum)
)
GO


if object_id('chicken_table') is null	 --鸡棚
Create Table chicken_table
(
FarmNum				int,
ChickenNum			int,
ChickenName			nvarchar(200),
ChickenID			int,

IsUse				bit default 0,
DisplayOrder		int default 1,

Foreign Key(FarmNum) References farm_table(FarmNum),
Primary Key(ChickenID)
)
GO

if OBJECT_ID('chicken_args') is null
Create Table chicken_args
(
ID			int identity(1, 1),

DateTime	datetime default getdate(),
ChickenID	int,

TempUp		float,
TempDown	float,
CO2Up		float,
CO2Down		float,
NH3Up		float,
NH3Down		float,
LightUp		float,
LightDown	float,
HumidityUp	float,
HumidityDown	float,
IsOK		bit default 0,


Foreign Key(ChickenID) References chicken_table(ChickenID),
Primary Key(ID)
)

GO

if OBJECT_ID('chicken_info') is null
Create Table chicken_info
(
ID			int identity(1, 1),

DateTime	datetime default getdate(),
ChickenID	int,

Switch		char(2),
Node1_Temp	float,
Node1_CO2	float,
Node1_NH3	float,
Node1_Light	float,
Node1_Humdity	float,

Node2_Temp	float,
Node2_CO2	float,
Node2_NH3	float,
Node2_Light	float,
Node2_Humdity	float,

Node3_Temp	float,
Node3_CO2	float,
Node3_NH3	float,
Node3_Light	float,
Node3_Humdity	float,

Foreign Key(ChickenID) References chicken_table(ChickenID),
Primary Key(ID)
)

GO



if OBJECT_ID('chicken_ctrl_dev') is null
Create Table chicken_ctrl_dev
(
ID			int identity(1, 1),

DateTime	datetime default getdate(),
ChickenID	int,

Switch		char(2),
IsOK		bit default 0,


Foreign Key(ChickenID) References chicken_table(ChickenID),
Primary Key(ID)
)

GO

if OBJECT_ID('devs') is null
Create Table devs
(
DevID		int identity(0,1),
DevName		nvarchar(40),
IsUse		bit default 0,
Primary Key(DevID)
)
GO


if OBJECT_ID('chicken_devs') is null
Create Table chicken_devs
(
ID			int identity(1, 1),

ChickenID	int,

Dev1	int,
Dev2	int,
Dev3	int,
Dev4	int,
Dev5	int,
Dev6	int,
Dev7	int,
Dev8	int,
Dev9	int,
Dev10	int,
Dev11	int,
Dev12	int,
Dev13	int,
Dev14	int,
Dev15	int,
Dev16	int,

Foreign Key(ChickenID) References chicken_table(ChickenID),
Foreign Key(Dev1) References devs(DevID),
Foreign Key(Dev2) References devs(DevID),
Foreign Key(Dev3) References devs(DevID),
Foreign Key(Dev4) References devs(DevID),
Foreign Key(Dev5) References devs(DevID),
Foreign Key(Dev6) References devs(DevID),
Foreign Key(Dev7) References devs(DevID),
Foreign Key(Dev8) References devs(DevID),
Foreign Key(Dev9) References devs(DevID),
Foreign Key(Dev10) References devs(DevID),
Foreign Key(Dev11) References devs(DevID),
Foreign Key(Dev12) References devs(DevID),
Foreign Key(Dev13) References devs(DevID),
Foreign Key(Dev14) References devs(DevID),
Foreign Key(Dev15) References devs(DevID),
Foreign Key(Dev16) References devs(DevID),
Primary Key(ID)
)

GO


if OBJECT_ID('code') is null
Create Table code
(
ID			int identity(1,1),
DateTime	datetime default getdate(),
Code		varchar(300),
Flag		varchar(30),

Primary Key(ID)
)
GO