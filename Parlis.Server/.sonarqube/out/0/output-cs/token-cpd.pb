†
ZC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Constants.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
{ 
public 

class 
	Constants 
{ 
public 
const 
int 
RED_COIN_CODE &
=' (
$num) *
;* +
public 
const 
int 
BLUE_COIN_CODE '
=( )
$num* +
;+ ,
public 
const 
int 
GREEN_COIN_CODE (
=) *
$num+ ,
;, -
public 
const 
int 
YELLOW_COIN_CODE )
=* +
$num, -
;- .
public

 
const

 
int

 $
INITIAL_YELLOW_COIN_SLOT

 1
=

2 3
$num

4 5
;

5 6
public 
const 
int "
INITIAL_BLUE_COIN_SLOT /
=0 1
$num2 4
;4 5
public 
const 
int !
INITIAL_RED_COIN_SLOT .
=/ 0
$num1 3
;3 4
public 
const 
int #
INITIAL_GREEN_COIN_SLOT 0
=1 2
$num3 5
;5 6
} 
} †!
ZC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Data\Coin.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Data  $
{ 
[ 
DataContract 
] 
public 

class 
Coin 
{ 
[ 	

DataMember	 
] 
public		 
int		 
ColorTeamValue		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		. /
[

 	

DataMember

	 
]

 
public 
string 
ColorTeamText #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	

DataMember	 
] 
public 
int 
AtSlot 
{ 
get 
;  
set! $
;$ %
}& '
[ 	

DataMember	 
] 
public 
bool 
	FirstLeap 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	

DataMember	 
] 
public 
bool 

AtFinalRow 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	

DataMember	 
] 
public 
bool 
IsWinner 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	

DataMember	 
] 
public 
bool 
	IsPlaying 
{ 
get  #
;# $
set% (
;( )
}* +
[ 	

DataMember	 
] 
public 
int 
NumRolls 
{ 
get !
;! "
set# &
;& '
}( )
[ 	

DataMember	 
] 
public 
int 
Points 
{ 
get 
;  
set! $
;$ %
}& '
[ 	

DataMember	 
] 
public 
string !
PlayerProfileUsername +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
Coin 
( 
int 
code 
) 
{ 	
ColorTeamValue 
= 
code !
;! "
	IsPlaying   
=   
true   
;   
switch!! 
(!! 
ColorTeamValue!! "
)!!" #
{"" 
case## 
	Constants## 
.## 
RED_COIN_CODE## ,
:##, -
AtSlot$$ 
=$$ 
	Constants$$ &
.$$& '!
INITIAL_RED_COIN_SLOT$$' <
;$$< =
ColorTeamText%% !
=%%" #
$str%%$ )
;%%) *
break&& 
;&& 
case'' 
	Constants'' 
.'' 
BLUE_COIN_CODE'' -
:''- .
AtSlot(( 
=(( 
	Constants(( &
.((& '"
INITIAL_BLUE_COIN_SLOT((' =
;((= >
ColorTeamText)) !
=))" #
$str))$ *
;))* +
break** 
;** 
case++ 
	Constants++ 
.++ 
GREEN_COIN_CODE++ .
:++. /
AtSlot,, 
=,, 
	Constants,, &
.,,& '#
INITIAL_GREEN_COIN_SLOT,,' >
;,,> ?
ColorTeamText-- !
=--" #
$str--$ +
;--+ ,
break.. 
;.. 
case// 
	Constants// 
.// 
YELLOW_COIN_CODE// /
:/// 0
AtSlot00 
=00 
	Constants00 &
.00& '$
INITIAL_YELLOW_COIN_SLOT00' ?
;00? @
ColorTeamText11 !
=11" #
$str11$ ,
;11, -
break22 
;22 
}33 
}44 	
}55 
}66 õ
[C:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Data\Match.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Data  $
{ 
[ 
DataContract 
] 
public 

class 
Match 
{ 
[		 	

DataMember			 
]		 
public

 
int

 
Code

 
{

 
get

 
;

 
set

 "
;

" #
}

$ %
[ 	

DataMember	 
] 
public 
DateTime 
Date 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	

DataMember	 
] 
public 
string !
PlayerProfileUsername +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
Match 
( 
) 
{ 	
} 	
} 
} Æ

]C:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Data\Message.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Data  $
{ 
[ 
DataContract 
] 
public 

class 
Message 
{ 
[		 	

DataMember			 
]		 
public

 
string

 
Content

 
{

 
get

  #
;

# $
set

% (
;

( )
}

* +
[ 	

DataMember	 
] 
public 
string !
PlayerProfileUsername +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
Message 
( 
) 
{ 	
} 	
[ 	
OperationContract	 
] 
override 
public 
string 
ToString 
( 
)  
{ 	
return 
$" 
{ !
PlayerProfileUsername +
}+ ,
$str, .
{. /
Content/ 6
}6 7
"7 8
;8 9
} 	
} 
} æ
\C:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Data\Player.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Data  $
{ 
[ 
DataContract 
] 
public 

class 
Player 
: 

IEquatable $
<$ %
Player% +
>+ ,
{ 
[		 	

DataMember			 
]		 
public

 
string

 
EmailAddress

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
[ 	

DataMember	 
] 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
[ 	

DataMember	 
] 
public 
string 
PaternalSurname %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	

DataMember	 
] 
public 
string 
MaternalSurname %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	

DataMember	 
] 
public 
string !
PlayerProfileUsername +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
Player 
( 
) 
{ 	
} 	
public 
override 
bool 
Equals #
(# $
object$ *
obj+ .
). /
{ 	
return 
Equals 
( 
obj 
as  
Player! '
)' (
;( )
} 	
public 
bool 
Equals 
( 
Player !
player" (
)( )
{ 	
return 
player 
!= 
null !
&&" $
EmailAddress   
==   
player   %
.  % &
EmailAddress  & 2
&&  3 5
Name!! 
==!! 
player!! 
.!! 
Name!! "
&&!!# %
PaternalSurname"" 
=="" !
player""" (
.""( )
PaternalSurname"") 8
&&""9 ;
MaternalSurname## 
==## !
player##" (
.##( )
MaternalSurname##) 8
;##8 9
}$$ 	
public&& 
override&& 
int&& 
GetHashCode&& '
(&&' (
)&&( )
{'' 	
return(( 
HashCode(( 
.(( 
Combine(( #
(((# $
EmailAddress(($ 0
,((0 1
Name((2 6
,((6 7
PaternalSurname((8 G
,((G H
MaternalSurname((I X
,((X Y!
PlayerProfileUsername((Z o
)((o p
;((p q
})) 	
}** 
}++ ô
cC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Data\PlayerProfile.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Data  $
{ 
[ 
DataContract 
] 
public 

class 
PlayerProfile 
:  

IEquatable! +
<+ ,
PlayerProfile, 9
>9 :
{ 
[		 	

DataMember			 
]		 
public

 
string

 
Username

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

+ ,
[ 	

DataMember	 
] 
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	

DataMember	 
] 
public 
bool 

IsVerified 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
PlayerProfile 
( 
) 
{ 	
} 	
public 
override 
bool 
Equals #
(# $
object$ *
obj+ .
). /
{ 	
return 
Equals 
( 
obj 
as  
PlayerProfile! .
). /
;/ 0
} 	
public 
bool 
Equals 
( 
PlayerProfile (
player) /
)/ 0
{ 	
return 
player 
!= 
null !
&&" $
Username 
== 
player !
.! "
Username" *
&&+ -
Password 
== 
player !
.! "
Password" *
;* +
} 	
public   
override   
int   
GetHashCode   '
(  ' (
)  ( )
{!! 	
return"" 
HashCode"" 
."" 
Combine"" #
(""# $
Username""$ ,
,"", -
Password"". 6
,""6 7

IsVerified""8 B
)""B C
;""C D
}## 	
}%% 
}&& Î
iC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Services\IChatManagement.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Services  (
{ 
[ 
ServiceContract 
( 
CallbackContract %
=& '
typeof( .
(. /#
IChatManagementCallback/ F
)F G
)G H
]H I
public 

	interface 
IChatManagement $
{		 
[

 	
OperationContract

	 
(

 
IsOneWay

 #
=

$ %
true

& *
)

* +
]

+ ,
void 
ConnectToChat 
( 
string !
username" *
,* +
int, /
code0 4
)4 5
;5 6
[ 	
OperationContract	 
] 
void 
DisconnectFromChat 
(  
string  &
username' /
)/ 0
;0 1
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
SendMessage 
( 
int 
code !
,! "
Message# *
message+ 2
)2 3
;3 4
} 
[ 
ServiceContract 
] 
public 

	interface #
IChatManagementCallback ,
{ 
[ 	
OperationContract	 
] 
void 
ReceiveMessages 
( 
List !
<! "
Message" )
>) *
messages+ 3
)3 4
;4 5
} 
} ï
iC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Services\IGameManagement.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Services  (
{ 
[ 
ServiceContract 
( 
CallbackContract %
=& '
typeof( .
(. /#
IGameManagementCallback/ F
)F G
)G H
]H I
public 

	interface 
IGameManagement $
{		 
[

 	
OperationContract

	 
(

 
IsOneWay

 #
=

$ %
true

& *
)

* +
]

+ ,
void 
ConnectToBoard 
( 
string "
username# +
,+ ,
int- 0
code1 5
)5 6
;6 7
[ 	
OperationContract	 
] 
void 
DisconnectFromBoard  
(  !
string! '
username( 0
)0 1
;1 2
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
GetCoinsByBoard 
( 
string #
username$ ,
,, -
int. 1
code2 6
)6 7
;7 8
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 

LeaveMatch 
( 
string 
username '
)' (
;( )
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
SetCoinToMove 
( 
int 
turn #
)# $
;$ %
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
SetNextTurn 
( 
) 
; 
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
	ThrowDice 
( 
) 
; 
[ 	
OperationContract	 
] 
bool   
RegisterMatch   
(   
Match    
match  ! &
)  & '
;  ' (
}!! 
[## 
ServiceContract## 
]## 
public$$ 

	interface$$ #
IGameManagementCallback$$ ,
{%% 
[&& 	
OperationContract&&	 
]&& 
void'' 
MoveInNormalPath'' 
('' 
int'' !

turnPlayer''" ,
)'', -
;''- .
[)) 	
OperationContract))	 
])) 
void**  
ReceiveCoinsForBoard** !
(**! "
List**" &
<**& '
Coin**' +
>**+ ,
coins**- 2
)**2 3
;**3 4
[,, 	
OperationContract,,	 
],, 
void-- 
ShowDiceResult-- 
(-- 
int-- 

diceResult--  *
)--* +
;--+ ,
[// 	
OperationContract//	 
]// 
void00 )
ShowDisconnectedPlayerProfile00 *
(00* +
string00+ 1
username002 :
)00: ;
;00; <
[22 	
OperationContract22	 
]22 
void33 
ShowNextTurn33 
(33 
)33 
;33 
}44 
}55 õ
jC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Services\IMatchManagement.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Services  (
{ 
[ 
ServiceContract 
( 
CallbackContract %
=& '
typeof( .
(. /$
IMatchManagementCallback/ G
)G H
)H I
]I J
public 

	interface 
IMatchManagement %
{ 
[		 	
OperationContract			 
]		 
bool

 
CheckMatchExistence

  
(

  !
int

! $
code

% )
)

) *
;

* +
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
ConnectToMatch 
( 
string "
username# +
,+ ,
int- 0
code1 5
)5 6
;6 7
[ 	
OperationContract	 
] 
void 
CreateMatch 
( 
int 
code !
)! "
;" #
[ 	
OperationContract	 
] 
void 
DisconnectFromMatch  
(  !
string! '
username( 0
,0 1
int2 5
code6 :
): ;
;; <
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
ExpelPlayerProfile 
(  
string  &
username' /
)/ 0
;0 1
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
GetPlayerProfiles 
( 
string %
username& .
,. /
int0 3
code4 8
)8 9
;9 :
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
	SetBoards 
( 
) 
; 
} 
[ 
ServiceContract 
] 
public   

	interface   $
IMatchManagementCallback   -
{!! 
["" 	
OperationContract""	 
]"" 
void## '
ExpelPlayerProfileFromMatch## (
(##( )
string##) /
username##0 8
)##8 9
;##9 :
[%% 	
OperationContract%%	 
]%% 
void&& !
ReceivePlayerProfiles&& "
(&&" #
List&&# '
<&&' (
string&&( .
>&&. /
playerProfiles&&0 >
)&&> ?
;&&? @
[(( 	
OperationContract((	 
](( 
void)) 

StartMatch)) 
()) 
))) 
;)) 
}** 
}++ €
rC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Services\IPlayerProfileManagement.cs
	namespace 	
Parlis
 
. 
Server 
. 
Service 
.  
Services  (
{ 
[ 
ServiceContract 
] 
public 

	interface $
IPlayerProfileManagement -
{ 
[		 	
OperationContract			 
]		 
bool

  
CheckPlayerExistence

 !
(

! "
string

" (
emailAddress

) 5
)

5 6
;

6 7
[ 	
OperationContract	 
] 
bool '
CheckPlayerProfileExistence (
(( )
string) /
username0 8
)8 9
;9 :
[ 	
OperationContract	 
] 
bool 
DeletePlayer 
( 
string  
emailAddress! -
)- .
;. /
[ 	
OperationContract	 
] 
bool 
DeletePlayerProfile  
(  !
string! '
username( 0
)0 1
;1 2
[ 	
OperationContract	 
] 
Player 
	GetPlayer 
( 
string 
username  (
)( )
;) *
[ 	
OperationContract	 
] 
PlayerProfile 
GetPlayerProfile &
(& '
string' -
emailAddress. :
): ;
;; <
[ 	
OperationContract	 
] 
PlayerProfile 
Login 
( 
string "
username# +
,+ ,
string- 3
password4 <
)< =
;= >
[ 	
OperationContract	 
] 
bool 
RegisterPlayer 
( 
Player "
player# )
)) *
;* +
[!! 	
OperationContract!!	 
]!! 
bool"" !
RegisterPlayerProfile"" "
(""" #
PlayerProfile""# 0
playerProfile""1 >
)""> ?
;""? @
[$$ 	
OperationContract$$	 
]$$ 
bool%% 
SendMail%% 
(%% 
string%% 
username%% %
,%%% &
string%%' -
title%%. 3
,%%3 4
string%%5 ;
message%%< C
,%%C D
int%%E H
code%%I M
)%%M N
;%%N O
['' 	
OperationContract''	 
]'' 
bool(( 
UpdatePlayer(( 
((( 
Player((  
player((! '
)((' (
;((( )
[** 	
OperationContract**	 
]** 
bool++ 
UpdatePlayerProfile++  
(++  !
PlayerProfile++! .
playerProfile++/ <
)++< =
;++= >
},, 
}-- “
hC:\Users\sebtr\OneDrive\Escritorio\Parlis\Parlis.Server\Parlis.Server.Service\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 0
)0 1
]1 2
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str 2
)2 3
]3 4
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *