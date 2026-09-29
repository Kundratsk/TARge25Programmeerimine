using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;

using Xunit;
using Xunit.Sdk;

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

            SpaceshipDto dto = MockSpaceshipData();

            SpaceshipDto domain = new();

            domain.Id = guid;
            domain.EnginePower = 1000000;
            domain.Name = "Igor Mang 2";
            domain.ShipType = "püramiid";
            domain.Crew = 412;
            domain.CreatedAt = dto.CreatedAt; // <-- ei tohi muutuda Update korral
            domain.UpdatedAt = DateTime.UtcNow; // <-- peab muutuma

            //tegevus
            await Svc<ISpaceshipServices>().Update(dto);

            //kontroll

            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.EnginePower, domain.EnginePower);
            Assert.NotEqual(dto.Name, domain.Name);
            Assert.DoesNotMatch(dto.Crew.ToString(), domain.Crew.ToString());
            Assert.DoesNotMatch(dto.ShipType, domain.ShipType);
            Assert.Equal(dto.CreatedAt, domain.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, domain.CreatedAt);
        }

        [Fact]

        public async Task ShouldNot_UpdateSpaceshipById_WhenNoDataIsUpdated()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceshipData();
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);

            //tegevus
            SpaceshipDto nullDto = MockSpaceshipNullData();
            var result = await Svc<ISpaceshipServices>().Update(nullDto);

            Assert.Null(result);
        }

        //kuna mootor ei saa olla negatiivse võimsusega, kontrollime et ei saaks lisada võimetut mootorit ega negatiivse võimsusega mootorit
        [Fact]
        public async Task ShouldNot_CreateSpaceshipWithNegativeEnginePower_WhenEnginePowerNegative()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceshipData(true);
            dto.EnginePower -= (dto.EnginePower * 2);

            //tegevus
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //Kontroll
            Assert.True(result.EnginePower > 0);
        }

        //Returns a nulled object for testing purposes

        //test mis kontrollib, et meeskond on suurem kui 3 liiget
        //service ei tohi lisada sellest vähema arvuga objekti, service
        //võib selle probleemi lahendada ükskõik kuidas
        [Fact]
        public async Task ShouldNot_CreateSpaceshipWithLessThanFourCrewMembers_WhenCrewCountIsTooLow()
        {

            // ülesseade
            SpaceshipDto dto = MockSpaceshipData();
            dto.Crew = 2; // Määrame vigase väärtuse (vähem kui 3)

            // tegevus 
            var result = await Svc<ISpaceshipServices>().Create(dto);

            // kontroll - teenus ei tohi midagi andmebaasi luua, tulemus peab olema null
            Assert.True(result.Crew > 3);
        }

        [Fact]

        public async Task Should_RemoveSpaceshipFromDatabase_WhenSpaceshipIsDeleted()
        {
            // ülesseade
            SpaceshipDto dto = MockSpaceshipData();


            // tegevus
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deletedSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)createdSpaceship.Id);
            var result = await Svc<ISpaceshipServices>().DetailAsync((Guid)createdSpaceship.Id);

            // kontroll

            Assert.Equal(createdSpaceship.Id, deletedSpaceship.Id);
            Assert.Null(result);
        }

        [Fact]
        // 0 references
        public async Task Should_RemoveSpaceshipFromDatabase_WhenSpaceshipIsDeleted() { ...}

        [Fact]
        // 0 reference
        // 0 references
        public async Task ShouldNot_RemoveSpaceshipFromDatabase_WhenSpaceshipIdIsDifferent()
        {
            var dto = MockSpaceshipData();
            var createdSpaceship1 = await Svc<ISpaceshipServices>().Create(dto);
            var createdSpaceship2 = await Svc<ISpaceshipServices>().Create(dto);
            var deleteResult = await Svc<ISpaceshipServices>().Delete((Guid)createdSpaceship2.Id);
            var spaceshipindb = await Svc<ISpaceshipServices>().DetailAsync((Guid)createdSpaceship1.Id);

            //kontroll
            Assert.NotNull(spaceshipindb);
            Assert.NotEqual(deleteResult.Id, createdSpaceship1.Id);
            Assert.Equal(createdSpaceship1, spaceshipindb);
        }








        private SpaceshipDto MockSpaceshipNullData()
        {
            return new SpaceshipDto
            {
                Id = null,
                Name = "",
                ShipType = "",
                Crew = 0,
                EnginePower = 0,
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MaxValue,
            };
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
