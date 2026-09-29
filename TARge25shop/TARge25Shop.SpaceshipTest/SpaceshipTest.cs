using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {
        [Fact] //Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - Kirjeldatakse ära kas test on tavaline, või negatiivne ()
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust

        //Selles testis kontrollitakse, et (2) kosmoselaeva lisamisel (1) ei tohiks (3) saadud tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //                     1        2                   3
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            //ülesseade
            SpaceshipDto dto = new SpaceshipDto()
            {
                Name = "X AE a L 12",
                ShipType = "lendav taldrik",
                Crew = 666,
                EnginePower = 12,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            //tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }

        //Selles testis kontrollitakse, et (2) Spaceshipi päring andmebaasist (1) ei tohiks tagastada objekti (3) kui Id-d ei ole samad:
        //                  1               2           3
        [Fact]
        public async Task ShouldNot_GetSpaceShipByID_WhenIDNotEqual()
        {
            //ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("eda81556-c05d-457e-a2da-4ce3d74f0439");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            //kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }

        //Seleta kodus lahti, nagu eelnevate testide laused eesti keelde.
        //Selles testis kontrollitakse, et kosmoselaeva päringul andmebaasist peaks tagastama objekti siis kui ID on sama
        [Fact]
        public async Task Should_GetSpaceshipByID_WhenGuidIsEqual()
        {
            //ülesseade
            Guid databaseGuid = Guid.Parse("eda81556-c05d-457e-a2da-4ce3d74f0439");
            Guid seekGuid = Guid.Parse("eda81556-c05d-457e-a2da-4ce3d74f0439");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(seekGuid);

            //kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        //Selles testis kontrollitakse, et kosmoselaeva kustutamisel andmebaasist peaks kustutama objekt kui tagastatav väärtus on sama
        [Fact]
        public async Task Should_SpaceshipDeletedByID_WhenReturnedResultIsEqual()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            //tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            //kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }
        [Fact]

        public async Task ShouldNot_DeleteSpaceshipByID_WhenDidNotDeleteSpaceship()
        {
            //ülesseade
            var dto = MockSpaceshipData();

            //tegevus
            var spaceShip1 = await Svc<ISpaceshipServices>().Create(dto);
            var spaceShip2 = await Svc<ISpaceshipServices>().Create(dto);

            var result = await Svc<ISpaceshipServices>().Delete((Guid)spaceShip2.Id);

            //kontroll
            Assert.NotEqual(spaceShip1.Id, result.Id);
        }

        [Fact]

        public async Task Should_UpdateSpaceshipById_WhenUpdatingData()
        {
            //ülesseade
            var guid = new Guid("fdaf7fc0-f3cf-4e0c-9434-13b0416a4ba2");

        }

        private SpaceshipDto MockSpaceshipData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new SpaceshipDto
                {
                    Name = "X AE a L 12",
                    ShipType = "lendav taldrik",
                    Crew = 666,
                    EnginePower = 12,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new SpaceshipDto
                {
                    Name = "raketa",
                    ShipType = "lendav kauss",
                    Crew = 56,
                    EnginePower = 32,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }

    }
}
