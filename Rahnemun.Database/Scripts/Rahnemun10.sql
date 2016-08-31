﻿CREATE TABLE [dbo].[Rahnemun_Payments] (
    [Id] [int] NOT NULL IDENTITY,
    [Price] [decimal](18, 2) NOT NULL,
    [HandlerId] [nvarchar](50) NOT NULL,
    [HandlerData] [nvarchar](max) NOT NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Rahnemun_Payments] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [dbo].[Rahnemun_Payments]([UserId])
ALTER TABLE [dbo].[Rahnemun_Categories] ADD [Price] [decimal](18, 2) NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Rahnemun_Categories] ADD [Terms] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Rahnemun_Payments] ADD CONSTRAINT [FK_dbo.Rahnemun_Payments_dbo.Rahnemun_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Rahnemun_Users] ([Id])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201504040549390_Rahnemun10', N'Rahnemun.Database.RahnemunMigrations.Configuration',  0x1F8B0800000000000400ED5D5B6FECB6117E2FD0FF20EC535A9C78BD3E4D901ED809ECF57162F4F802AF93F6CDA0257AAD46978DA475BC28FACBFAD09FD4BF5052A228DE45EAB2EB4D0EFCB2E665861CCE0C87B74FFFFBCF7F8FBF7B8D23EF05667998262793D9C1E1C483899F0661B23C99AC8BA72FBF997CF7ED1FFF70FC31885FBD9FEA72EF71395433C94F26CF45B1FA309DE6FE338C417E10877E96E6E95371E0A7F11404E9F4E8F0F0AFD3D96C0A118909A2E579C777EBA4086358FE83FE9DA7890F57C51A44576900A39CA4A39C4549D5BB0631CC57C08727933BF09CC0789D1C9CA731089389771A8500356301A3A7890792242D40811AF9E1C71C2E8A2C4D968B154A00D1FD660551B92710E59034FE4353DCB61F8747B81FD3A6624DCA5FE7056A911BC1D97B2298A958BD937827547048741F91888B0DEE7529BE93C91C147099669B892732FB308F325C5092EE415DE79D27E4BCA3CA807406FFBDF3E6EBA85867F02481EB2203D13BEF76FD1885FEDFE0E63EFD192627C93A8AD816A236A23C2E0125DD66E90A66C5E60E3E91765F06136FCAD79B8A156935A64ED5A3CBA4787F34F1AE1173F01841AA004CEF17459AC1EF610233D4DBE0161405CC124C03962294B80BBCE660558D58C510291D329E8977055E3FC164593C9F4CBE42D67211BEC2A04E206DF8310991A9A13A45B6868A369AF99EC3DCCFC236DEB3C3C351B8DF66A10F6BBEE7D00F63104DBCDB0CFD22BEE49B89B7F00126A892BF99FA3DCCE2DCD0AB6F46EAD57998AF22B0B9C90298D5ECCF36853BA1DA70BECFD2F5AA5D1B0562D7E0255C96CA69223BF1EE605496CA9FC355E502A9C53E08452FB234BE4B2386065FE26191AEB372445363B17B902D61C1B7F878DA381B2B1744DAE4EE87CA8A9F9DD11B7446DD0CC75E739092A3210549E1A436B4D6603AE39D811C12E1627B43614636318A66B1423E114478ECB63C286720F9790EB2E03A354D105F8FC5FAD4F75314E819B9BFB7EB783B33FCABBF80CD7C3E066B1FEC6CBE4D9F608E1516441F5F514E88E274538FC76AC82734EF2739BC5EC78F8DA98F35B26CAFE7283D7C0AD10840536860DD6F33E7BFA7D9CFA7419021EEC64E8E2263CCFCF6394DA0D176C6711A6806017ED878AB4EE1CFE96A95A52F904E7C67298A2940E2DE18EAC0AFE1AFE7A0000671A09F038C7B1D690C1FB3A9C335DAC187A61C13ABC9D972A0A628D32B4ABB423A0F96D065A225553E47667A5EF7F0B5D8C18AE66CB328405630515947535CA06EDE87CD248BAC1156FF3B1382899A90B9DE150C42208D581BAF72F218CE980941A52D132378A0651A3B16B2241B16F37BD92FE56F6FBFA4CA67FB35A81236A3610CA048575D0CA0AC876A502F82958E8C5C9367A6718764813D0F3BB75BF41B66CE26240E55F2121610F625B3E5E9B9B647D5DC2CE649462D155059B5A9694466C696D13272C34896B65D75BE6BB3E8846268162D23378B64699B55E7F7F281E5B2DCC101E2F2BF47EF778D1619AD910BF24D7901E215751B6102B02DE0739B7596A185E8E6AA8C50CAF0850F6BD4314D8796CED378B52EA02C27A1B5176196172DBB01966B53D79531D81163242C66DBADB4C33AC99112DDE1F8045F60C45114B31C295F977541D428AD72ED3EC00AEE0A64610122E4468A75CE7541C8719D7A6014AD5A17E5B3D9101B5A61563CE3B0A077AC710797618E0C68106257C8300D5D3F1A66E705EFF98411BC0D7DEC698D01BFF574700B363174DB3B26557E8F93826D483CEE19E00F20092236F0DCDA1618E1DCB2DF34D68A1D8722C385B75520A488D4887E3F54059A308D4D9762342EB3DF22152D58EEE02F6B1459382D549B6A9F2D53CF8B886890E5EA1DFC27F435A4DED0EE0BA319AA1D1845B6BC0251947153F275CCA8781D835DE617115836778F2CB41D05BB719A90288E51F42ABDF3D9E58F985CB4419E8C59FFF3B2BF82F85C878652116AEF4F205AA37F0EA571E2CA5EC0982D3D93455609C7203031C4EC2E389ED2EE048866C8182D9516FE739A46B692BC0A0334FD087566E63A3F84CB67A186BC392A340D9E01FF19466946ABBC375791CAFFA5A523202FF70048E9AFCCA5CF531F39BD324E2515BEEEA043428CDF5D853842BBD3A0052A696F84A8D15988D7C83DAC50DA51EC2E4381D4C852E4BB7A9AE7A91F960D1476FA84AB51BC383F268167774FAA595736BB8357A8F5E10AB517CD0E27933F4B83D54ABD9E6E14D4C9A5299EC56C22062B37C9398C6001BD53BFBA873A07B90F02799645BA10F02928BE81190E3040840F1591FCC3A49083A130F1C31588AC3A22D4B60CA670EB281F31E71CAEF0B4981456E364D300E9B29EDC1ACA5490619BC88EA78C1EB6A8A7E22858AB3DA673614675985B530EAA693850B6D07A79A6EAA657FA566C43A9F40276D1A89D29937470A11B6CFD298674C6E4A243DAB38FED2990AE095BD01E9D50F74A75EA93A5B621968E9906511CF1704ABC7239AECE08DCB7A8328234AD383747AB3B5598FACCAF6D68A503C04114463C36DCAEC208DCB7A83082346D383347FA3B5118F1A28C6E68B5B76698239DFAAE9ABDC2E8EEDAB4ABE1EE836D4DDBB7A06E9AB1B052B7661B7027EAC6ED74EBB442BDEDDDA8043D41B25734E566F976DC928AF516944425441BB6F591C76EA62FC5C6B176B231ED2233FE833DDC7098CA0CFBCFFBE09D0CEDDFC684A81F9BB7E7A5AA030454A74035E8F65DBD3D864F1E715E796558DA6E43D64276DC7272C4282A14A6BD8005BFA208F1B389E6E0425A84496AA9A4526D929828917DAA1672643E5111A2737A0B0932622A12D4445A4860C7A3AA5FB9E696CAC4DBA9EAD3D9A2AD0B8DCE2ABBC1FA118114A358F22089AF3F99C2E677A2A2E65B6F87D29EF1FA269992F50EA84C90AA9EE8BD7851D88849F5EA4221A3B61D39EB3D39A63344E54C8231ECBF5989B98344E48BAEB238CC7B4A76BB4A4CFB1BF335C842BB8F34B620E8BD5ABD1C941B24565B245DA5206E8AB4A9558FFED30BBCFAFE2BD7FB562BFEAEFD17D7F823F45F7AC421F7DFB87CB55AC032ED6EE64143FF754B560B397610017F3348EEBF7E3DD5BEA2625ADC4C9F869E2BD75063A8BDEAF68842F5DBD60AD6AB0576E8B820C0640686F5413F55A86FB4D08094E61D4F2B0C1E92703CD580F51C5F81D52A4C960C780F49F1161572CFFCCB853BAA4D5CD198FAB902DC86B69672C217089650C8C5336A00CB7BE138B07E04F8107B1EC4523155F8AD89DF6A8E52842D8F5E1DD3D555F06F3EDC17512E14EB1552F902750FDB43D953A80891E4AA1E46510211C814D7D1E669B48E13FDEA4B5F9BE24CB02468A23D1D0EC486A5C565D8D3235752594A24C99E06019F61699024877E7178185CC7B81C17890B87D4BCE45B4EB0B1E50A3A242D72255595360978E577318D3A761FC23C942B15071BD1D47FE386D259A17634F0D5CCDC6BBC9BDB0BEE835D137B68A848AD196BCC19C4179608936C4F8BC5706189B1E96ED4185816912093E546B37AF62492AB52ED2931F82A2C2926D9652250E3A5F03383BA8C3D17010C85252E64756B398F79A26B3B5FCA9E138771C212E732DCE851D812911ECD70F29D048A44709E24D59E528347C2526A521DDA242392708D93B3DDA778DDEC3ECEC4DEE2952F73FCFBE6E90BD593D93F75F0CCAA65A35E26FD3C31F374950BE79A647B5ACCC352961693ECE037808A54936A4FA97E64C0D2A9D33A785E72F35EE97E499E3D55F6CD274B914DB7A726DCEC66090A590E16C7BEE9E44C8ECD70980B9B879BDC64D824DBD3E29F6EB2E4F81C070996EF3739C195296E7313FF36539C93F8DC37138DD26DB63E01697D30E5EEF3B43547727BE5C121BF807D958E904C1418D8204E919B64877098220771D1304D75A1544307F194EA540753A8C184386BA8135D5A444F8DF926690F93776602745BB08F0968363D2D4C405B73A4355803D7C30D4C93EC42AB06ECE149D5A98E94CA4735122505788F89520DE4C3CD0B24CD514AD5DD1B494AEA2B3986F16AAEA172C3A6BF9DAAA7B59B48B8A361D1838C3E8655DF1870372C6DCD710C6B881D56E6693F4B874976A6252FC5B80C7B7AF55D349694EE7EDAEE7C397B64D4CB9F33374C3AF87453ED71D48F7BD9CE07C54C860BBDE6753B4FAE49DFE370403AD8138B50EEF4804F38C83B26876AED9FE6904ED9AA221811247D09037CC2B6D8A0754B7C800B1C2C7E89E65158DE92AA0B5C81247CC283583EF59C1C1DCE8E840F7CBC9D8F6D4CF33C88148792CC4B5ACDE3AB2D204C8458AAAD1812EEC8C02CF875F20232FF196412204BFFEF58284957C0CE7D3E53118CFA998ABAD15FC4E0F54F8300E8A3686E538EE4301F9F089500489749005F4F26FF2AEB7EF02EFFF120547FE7958DFAE01D7AFF1E0CCEDFF002FBB3790CA4186E6089BB1904578313A108B3F457F2F52AED8036C0818ECC242441E590BE771F5211287028BA3C0E6057EFA1C600EC4A4DC6FDD3387796726798BFCE1E53C6F6533773E6DA4C09CA2F40BF8BA1A0FC3A1363A1FC943D3D1266DB3EC87DC288C8A49CA1E97F1B33060B09DF2F7A9050DE1F4377231011DE3B2B9788F0AE226465E23CE27BAB1AA95A222090D9C640B4E218D18FFA95D9BE6AB1048CDE5D6F0460F4AE7A2302A52B66051B323C567A4722127CBAB506D6151D345031F622ECBA2D7B5AB1177B19AEDD7511328E052AF74CF7D602C75DE44A38AC43AD6C1430ABFD66411E39D556D3AA5AA3F879DDF6E8DE6A9A0257B4473C2BE28A76F6F76F72966FBF81BA3535B0F1F7FD1CBDF491C3A19C84FC0D43F5C2ECEB613E5168B114EFF245420B6974FA00E1901BA5A6EF0B0EC747F9F9C081A4AEBF13EBD08B8E1F07D474C15D408A6FFF0D654BE2A7FDBAEE96885FF7EBB2D8D47ED94F1B0274FB7ADF4EE2CDC1B047C9F37D3DCAC2881020726B3A829EF68215D53DF6B19927BD2E30A27B84186A0DF2E83480320491F410BB1B665B2F3DD822A4DA6F0BA6B17AC5ED0C3BB5CB3157BF10D8ED78EF0BC8E2E7E11E64B8F70522D161B4460A0E285A930AC169147D7132DB41500FDF2080584798C3DEEE81225BA9D0AEF6DA3DE86FE0CAACF607B070E7FE81C330D3619BEDBB9F305FA37D7BBE828118C343D2907BC05F4C22683BE983C516E680AE45C98D25A82EC033383C38180843755B7EC70EA9C09AEF5BFB52C5504BCF41B4630F17A59DD563CB2B54E19E3955151179491C5F35CAA811A9B4BA577E32091EF11669B53BD788498F06A86346F6D2B41C49BE0D5B1D46A5C8BAF28D12C72AD9C8488D7826D2A711B8C482E618B9E860E97420AC260C5623232D5899C888069912239A6364A4839B3361B2B641B29A7B6680759398AA9D97C45D5DCCAC986C49A115BBC290E50C4E81CD24FB2BD584A2A82E47A6BB4642756AAE3A74573CA21DAE9B83E09CB2DEACC1F8D87DE70601317D6B9D1B14A1D4A1A9EA5D1F05C841FF2E0E8440EA3E72C224C3BDAF1D402D470119ED38828A694E7AD3D9BFCB7D169A6644EE7EEBD69E7A629AAEF5F073838AB39E4366032097779EA3C69683032EADFC4C152D66D609BE4F56FD770EF370D99038463413E873CB185AE632794AEB1595D0A2BA887457BD00015AE39CE26B27C02F50B68FAD105F20261F63FD183FC2E032B95917AB7581BA0CE3C788132F5E9599F897E0BB7C9B8F6FCA4767F9105D40CD0CF115BC9BE46C1D46CD47642F14171F3424F0728F5C37C46359E06B87CB0DA5749D26968488F8E82AF51EC6AB08DFE5B94916E00576691B32EA4F7009FC4DFDDA584FA47D2078B11F9F876099813827349AFAE85FA4C341FCFAEDFF0182E1B1256EA10000 , N'6.1.1-30610')
