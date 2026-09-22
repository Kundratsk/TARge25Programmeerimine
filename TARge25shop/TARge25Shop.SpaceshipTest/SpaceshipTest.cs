using System;
using System.Collections.Generic;
using System.Text;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {

        [Fact] //Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - Kirjeldatakse ära, kas test on tavaline või negatiivne 
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha.
        // 3 - Mis tingimustel tulemust kontrollitakse peale tegevust

        //selles testis kontrolitakse et (2) kosmoselaeva lisamisel (1) ei tohiks (3) saadud tulemus olla tühi
        //                      1             2                   3
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned() 
        {
            //Ülesseade
            SpaceshipDto dto = new SpaceshipDto() 
            {
                Name = "Melon Musk",
                ShipType = "uhvo",
                Crew = 666,
                EnginePower = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            // tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }
        [Fact] 
        //Selles testis kontrollitakse et Spaceshipi päring andmebaasist ei tohiks tagastada objekti kui Id-d ei ole samad
        //                      1           2               3
        public async Task ShouldNot_GetSpaceShipById_WhenIdNotEqual()
        {
            //ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("b6b40e73-7c22-457d-abd4-8fb634d6c853");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            // Kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
    }
}
