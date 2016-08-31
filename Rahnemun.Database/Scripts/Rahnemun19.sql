﻿ALTER TABLE [dbo].[Rahnemun_Consultants] ADD [Fee] [decimal](18, 2) NOT NULL DEFAULT 0
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Rahnemun_Categories')
AND col_name(parent_object_id, parent_column_id) = 'Price';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Rahnemun_Categories] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Rahnemun_Categories] DROP COLUMN [Price]
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201510120810389_Rahnemun19', N'Rahnemun.Database.RahnemunMigrations.Configuration',  0x1F8B0800000000000400ED1DD96E24B7F13D40FE61304F76B0D648BB5EC359483674AC6C21AB031AD9C99B407553A3CEF631EE43D620C897E5219F945F08D9079B7793EC63A4B5A0170DD9AC2A168B45B258ACFADF7FFEBBFFE35314CE1E619A05497C30DFDBD99DCF60EC257E10AF0EE6457EFFCDF7F31F7FF8F39FF63FFAD1D3ECD7E6BB77F83BD432CE0EE60F79BEFEB05864DE038C40B613055E9A64C97DBEE325D102F8C9E2EDEEEE5F177B7B0B8840CC11ACD96CFFBA88F32082E50FF4F338893DB8CE0B109E273E0CB3BA1CD52C4BA8B30B10C16C0D3C7830BF060F318C8A78E724894010CF6787610010194B18DECF67208E931CE488C80FBF647099A749BC5AAE5101086F366B88BEBB0761066BE23FB49F9BF663F72DEEC7A26DD880F28A2C4714D901DC7B573366C1377762EF9C300EB1EE236271BEC1BD2ED977303F0A93D55592E5F3198FECC37198E20F05EEEE346DDECCB89A37441890CCE0BF37B3E322CC8B141EC4B0C85310BE995D157761E0FD0D6E6E92CF303E888B30A4294434A23AA600155DA5C91AA6F9E61ADED7749FF9F3D9826DB7E01B9266549BAA476771FEEEED7C76819083BB101201A07ABFCC9314FE046398821CFA5720CF611A6318B064A1809DC37513E4216CD021914353673E3B074F9F60BCCA1F0EE66FDFBF9FCF4E8327E837253509BFC4019A69A8519E165042A21EEDB28822906E3488F7767777C7C08CA66B8E58A3C18CFE1D03712950D9C30D521D0DF2133464D56FDB4EA086AB44CBBF9106EE06AC320DD6F77B6F8DB0EA9120ED97764F82AE71460BC355E0E159DD17D6CD4311DDC520085DE15D80C76055CE57494FE7B36B189695D943B0AE1603A2BB6EAB2F4ED324BA4E424A119615B7CBA4483D2C4F89ACF606A42B98B3D4EC2F5ADDAAD5B81562736D8BBFFF236ADA8B24EE162034C9B31C44EB06DD511097EA0F6F1E8A3445BB96CD79393DCAB9C34CA9EFE513CA81D2E3245A173914F9C4517B1AA4598EFFD54CF477A328E74F604B8811B37C2CEE355A3C039B227DC38F7EE1954CFF041F61C800E0ABF4802ECA4F41D88AA474511C40BB9E8334C841B844C252640CC55C4D87828561B87E40C27F91E828DE1B80E2A320CD1FF062292E9CFA86D770156448F8E56DADF72C7799970677D0BF492EE0EF5908F1BC22333A410A18C4D650CFD154D52DE3A6DBA08ECD479ADC072154AD5F4E0B04D25D19D2E520B6DA94B7AD065B2C664720839418972B977E28D7D043A7AA5CB7817A3F8AA23902F1E76390FAFA49F3DD58A80F3D2F41C7572D76430DDB8DAC43931B32D850FF6EE128812715CCB2526D7F7C4235015AC7753D1E8B904F8107E30C5EA05D2AB5888D34B274AF8F5179701FA01180BA538171BFF598FF9EA49F0F7D3F45D8B59D1C85C718F955E772378ED238066BE005ADB63ADAE4F6400ED7EB149D89FCBE0B56ABC0D12288165330C051BA633F0ADBC51BE9ED0820957F95A2FF6ABB1FDA212F3D803BE0704CAC4FD2C31DE9DAB3B9E458D732EFB6FDAE3DDC49AA85239EEC9B5E07BD96108B55BC6EF3473CF0999AD6D0ACED5898C6D11627106F53BB708FB516DDC034D2A9E7EF47C27B1264EB106C2E53EA1CE7A4271BD1FE294D8AF5F06AA1042BD70DCD6CE63EA5D483F40B5143C83F1B4449D434D96B8AB2E1ABBA7886EAC26DE2984B4E292A303D47DB36B08256B2C3367D951E352EA9C9BC635B85A819C6D8D67144434315EA4C1C8637159DD6997F42AFBDD729352A2B3FE4135B134D23B9FDAD349698F1A81EAEF4D755A6572EB6D62E84D5E1AECA582D38A8835735D07D9D0B9F74B232D6E6EB68B3CC413A8035D451EA6480602C07D435D7FD00582AD265650E196EAF580394EE12EB49704BBE69B7875C95B02FE4EB7B6D08097EF3F95B37799DBF1A51C2D368980990276B970950B6432D9885B41EB9B6AEE3DA05F1026B1E7A3369D0EFFE97FE67F16390C3DEF7FDCE4621DE540A36111AF7896C4BCDB4961996F83A4137081FC894838EB49AF55ACAC8372261759592AEA6DE96AC7A08B464916F44B2EA2A25594DBD2D5964B9D49045BE11C9AAAB946435F5BD343C618BB986AF9BBC6A7835AEC3085FC08D66CFFE19C47E482BD2C9EC0835E68E1B81B176A043B88C0DB2EEA29F8F810FD3292E3FAFE16F05CCF22106DB0891CB7E02CD4C88FD9BE064545E437C3FA24336002EA4CFAA8B4F34655DD842B71F87607B9FC36631E35D0EE97261C9612AFB9D28D0EEB21E42AB5345DBEC75DD719CC2963AEE1A623B9AD3E962CAA3322519B2E3B2A45ADC5049BEB113F222A244BCF1293CCB4E43B06A1F741848FB711245495C7B2552825E953BBB4EFD82C1851BA471A8C31ACBFB7388DD4A88F3207E2EF02B080BF463571827E6DB5318D15FEF892CAB98A36118EF43E9CE3816D2F618789506F8E5C3D27B4892D09493E7818FF6565C9B3D7D9B9F83D503D742B46471A4C123E03DC03049499377FA26C2F7DF76740464E591A6FEFABDFEEB93C4434AAFF4DDAC1B7CE720439C57ABBB083180B627414BF4A5F9244444A701F6FAE9310B55F728EEAC9443DC1E4F2FF3074A2C3B58BA2C562BB42094CB89D95C5C16EB7592E6A613F11442FF0E789F55B3D068D0049B9DFB6871A0461E26B6AB875996784149206704E3FC24587E7E8CFD9999D304755D480C67E788FA608DE8454BFAC1FC2FC26875426FF60812E8B507058B628F251FA1B88C4F600873383BF4AA1799C720F3802F6E8D902CF86C497DE4C2EF46B1B318E27F10E7E20E3688BD600D42A38E70AD0D77C0983A8287AF39816BBC978973A371322140F0DC11A92148391E76B16C7F41C96187784A5CFC94D2A3F3F7A34487F2B4B7104D8DA3A081D48B2ACD4DAED4544C21546A06DB48D4D684897D06A81A69C59BC0768CDBD7D3E6E2237F4AC83FB3184768A4B827101729234DF036E6C7AD088970F1A31A52F52D9070D5672329CABBA3E9B48C8A84096446C5D417A15FF89BB9AE2116AEE906111CFE726F1A25A3C03EA1C870DC34C2DCDE706F4560181BB16A5CE506E37650C9CDA2B9A848CDCCD3C8890CF504422263E28B598A9A11EE9AF9C2FDFA20FA84BF95EF16BD81550A47C0842A8563A80966CA4B65AB32D3F83B748DAEE0FC3088CCF02E13D3AE411CF6090586E3A60966CA596B2B02C3BB40AA8656E90FD90E2DF142361718951765B7186EDFC8A3A07D0271538C8591B8B57786DBD14F926B44A536D1DD295202425F755BE82ACD6DE44B103F0DFD53683CF5D83C3F31ACAE9371B032D4825C0C347677EC645506327B92B94AA0F5AA36E567B56F082F5018F612E69C91299BCFDA5B6CC1722448250B042F923200D5E2D9D1B83E07075006A1351D9841A9ECBF3A48B509BE0B1C7B472505C8BF6DEB00A901650AA21643190832EF3B40D41B3E1908B23DEEA2A29D4B524A68FDC681A2045E1C3AFEED2BF5B1FE952C3F238DEF7F48CF582914A6B8F1958F08900824AF55595698B049163E40C2A3AE2B08E34B08AA33F504D7314673E160C466078E7071F2445E682CE80636748A6C4A4B6A7820B79A77B1D1A1E3A2B7BFD877BD61D8CC344C91DEAA1E0D0394C6E0B12440785CA0E683D4CA6964E774E5026FD91C411058AF4DB1F36A8B5DB7CD8E22B75D3434DD965AE946147EB25CA9C75C6A8932B245B98E396F7D3260620F169057226A16480D2B46A6155716F0C694114440780729F65F6B2730B2145074B75B374DFF55B601033EBA8880CCBF5522065DE757E3132CDD0D6603A81309CD99B51F5B1A9F5B72482275FB8B2AF47A5DB0BF50C468DF3F07EB7510AFA898ED75C96C59056C3FFE66691FCC3CAA602CBC4C12D39C504B306117C715E46AF16ECA876524567CD8BB03D863EBD88F84CF644742C5DEBDC1C89FFAC4C16BB6F34D0BFC3F7B02E563AC4B8ED075E353D43BACF4CA8E42C9864A6C3AC3B1F341085289BFFC71121651AC3608A85BD7A1CE69007591390C12B79C86420ACDE19028E4341C52680E87092A4EC3622A2CE822BB498630C5095CCBED325038C3ECB2C41C42739145C3505D6EE938CDC6016719CED659F44E12119CE9ABA45E84BEBFE0E688605712A6A2609763E7B6D1CCAF96609B597F96E1FF2FEFBF9205FDFEDA61EECB36A36A6EF79DF724F8363BF749B1392C2A34360D8B2A3687D506BBA641B5A5E6909A6725341C79F86A1D14FEAD050D4D1FCB5A07958E6B4D43A4CBCDA171BEFC34406DF06A1D4C269035A31EE80A737854B46A1A1A556C0E8B0D604D83636B6C563079146B7641937F63314E65546B6678CA128B954D8858CD2C6F42EDF0DAD572274439E9DAEBC306D86D0B45D0D06A5EF5D38F54306C4608DA620BD9A7C25B33C24F95DB41A32256F300A92A3B98A2D66D4B1DF4A54255DA4ABB2C94342FF3B26F2C561C364E34B3ECB0556E94B3E1A055B4B35F996362C23FD3C0990A3B785732BDCF54D8EC9D9B28CDECDEB9293587D4866AA621B5A556E70C3E583377E2E0AB2DF64290DF0541A7B386B02BD738C56E6DCF4C99AC7B2D120A4BBCC112A16E3ACEB240E29172D26CABDA9870C4342CA6C2623B5F051866B6F25591054D4CCC538628A6C65E98C9DB2399442B1F266D5BAC9B1BCA21445B7A1F6B21DF8AF6CF5CC89D056A5B03CF3B37F41A7ACE0DC261F0BB208C33FCC39898DA40BACC6A484A2DB69455A85C663B5915591DF0AAD7D9DC89AE2AB438C2350E29CC294EEEA5D2C5E33A862DCFE6BAD8A26FB0090FCA748E943E9BE935C8B4729F4E134FA32AEA2CBB223F590D2C1540963996B5C5630A891A5213449685D494DA4CA63AAC2C3B99EA421B8A8813224B92D237716B5380DCE8F599028AFB4A8329A06C399211A50DDCCA0C4C5B6C03AB09DDCA826A4A2D2195C11F04489230AE3A484D4857C60459975972495C72350F0A34E3D5BE8463864DFD404E0DCBED28AA3489B40F6A181B88FA9DCDD626297148E93349152E36069354D9729C49DAC4DE64AC2A759939142AC4260D882AB686255A66988AA937B4A2F2B1553C6C144CCE1248D5D8DC84905897EC350829B686257693A9B08147C5B864E15115D6F435712125143655E630C580953458B1D60DB28C6459FDB351808C1F53AF9D0AE5F2EEB05BD1B51E47190E3F09DAA0902CB8B6FC056F74056F33FE1382BD2E21BF89B759EDE9C5B8A095FDC70E6565BFB3DAEB8C77FDAA3EC151A22BE58918B1C97218EDE00F7696BF85C761501EA59B0FCE411CDCE3412C836DCDDFEEE22431876100B2CA29B0766AFBC0BF1D32F272DB7B87BDDCA01F2DF8E6F6BE72184A96F9A1C4538E8A652677159B22306B80B9DA197AD53AD074E9765661881F41EA3D8054CC89649D9FBA7643D3C0ADB2FED967FCACFCD258C05F45E0E96BFBF8D8B4635A05D0475CCD5DE2D0B67E6903B3B27255D3002D932EB5404D2246B311CA036960FDB3D8874F07F37F954D3ECCCEFE715BB57A332BEDC71F66BBB37F3B0C1FEBE5461160CB1689579B2934E330D092C82253CD738778F1B5075905244D7E7FAC02212A75C67112AD8B1C5FE25A22A35CCC3472F9CE7E76B70E67C3C26D1CD02AA879106F381E5BA5F7AEFDCE7A01A35DCEB43AD21630E77AD68B48C6E94C47E59E2D64CA014DAD77CDE2ECD3AE673D75B8CAD7AC027B1738A4EB2B3DCD742B02B7089A663D671DCEA47A4304659F86F9CBD8E370E94CE5ABA8BD569124371E6EA3C3E42EEEB7CD9165559568843E19894DB7105C738BBD846376E057011E45309C33EE7E19E321D93FDBA9703EDFADC146C73ABDADC909C4219BADABE6E032D71AAD866E89694D4E494E19400D3716D67967BF8C4941E77BEDB76009295C5DB65E8E832703C4A66F75DDAA72E95C1DB48690B1C674D9250DC75870E571A85EAA140B594FDDE586CB7AEA2A377C1654C7C31C9B08D51188901BD558029B86BDCC47424E5553F4A4613FEB95908BD576DFDB93002189AB297ED2700C05200FB0FA5215009B14D31F3D29E6509B7949CECB7EABF0D68CC48368DF2B49EE4B03563BA5BA1C16EE106B8624D3E5B044B27922E5B06D41ABF258BA32419DD7D2855EA7DC915F864A34144CC77C8DCEFBA267B91BEE7E3D3B991898EC8BFA696AEA5DEEB08B19FD46576759FDCE0932F5587750F34BFB727750954BBDE21DDACA7CA578C73B341EEE49EFA05CBF52BEE7B5E8850922E66DAFB60BF60C62DEF90E3B97DA57BFFD0C78ED9B5F77A38CE4916FC756D5C89A0AE168FBF589CF7CAA7566B0D4837530DBAA2B13270314A971CC79D82BABA0EA51A8C91A3C73C922F88212069AE578ABC27B5223679695428C4CEB9818CE69F8E5619EC6CBDBF67212E474A786B499ADBA712791212938C679527A4DFA09D3987C59D9D67ACFF5E9C77CAA996E33DE2F22575AEFB12631902918C609D69EF558ABDF953D53A5DE95F5CC62A89ED7ECB619890927F84BC95CF6AACF0719EE979277CC62B4463AEA717E3F76A9CEFA24B29B3095D8334CDAD43377D8D6C58649DB43536193AEECB98B8FFEA1E6F313212AAB0E1E9216DCED699A4475AA8DE4D6E00E64C005498A8D0628FF8045B0BBB333501ACDA9562BB330ADC678A7959EE94C0E8348C70B3446388BC7C49609EE253311153EE1043FBEF2B481DAAC81D5CBE583B97F87EF582A133C619332A59634BBA032B9A016873CDF0C0FBF957201475BA5C5A3C969A542561BFF9518EB7A13B4AA1473026A3E61A1889CFF428F5EC891D841801AB11942634464E7242022355A44CA84343C2272801710911A2D22654E28A147F42E4CEC155DABEF9926758F8054AEA905ECF2CFF4A2437FC951B1AD1C91CC8C9444251595B36CF5943417B7E1DBCB7448EBD2361987AE6BBCA667D3F7F4EFDAC0B90CAD46427E04A343B328B2AA6E3953A1FD288EDBB981D210DA778BD3BF4C5CB1E1C66C904C8316A46E412C07C924F8DCC472D0348116A4CACD71543B55BEE7E79206D0B1AB924D8A10E0AA7F97FBD844F4F992FB99587A4E05DD664B9D266650760E9856DA79151E9B0D168923C5905DE8D85DC4D875BAFA7502B360D582D8473063E831076EF2CD597C9F34677F8EA2E613E1F9620E7C741A3FC41E96C0CB51B58727217E53F62B080B889F1FDF41FF2CBE2CF27591A32EC3E82E64D88BED073AFC65764C96E6FDCBF2E97B364417109901F636BF8C8F8A20F409DDA7123F3C05086C98A83DEBF158E6D8C37EB521902E92D81050CD3E624FB981D13AC46EAB97F1123C4217DAD09CFE0457C0DB346F5FD440BA078265FBFE4900562988B21A46DB1EFD4432EC474F3FFC1F72F5182F06D30000 , N'6.1.3-40302')

