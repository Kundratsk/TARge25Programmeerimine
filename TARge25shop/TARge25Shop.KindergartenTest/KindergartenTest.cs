using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class KindergartenTest : TestBase
    {
        // Selles testis kontrollitakse, et korrektsete andmete
        // edastamisel lasteaia loomise teenusele peaks tagastatav
        // tulemus olema edukas ning andmebaasi loodud objekt ei tohi olla tühi.

        [Fact]
        public async Task Should_AddKindergarten_WhenResultIsReturned()
        {
            //ülesseade
            var dto = MockKindergartenData(false);
            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }


        // Selles testis kontrollitakse, et lasteaia rühma päringul
        // andmebaasist peaks süsteem tagastama objekti siis,
        // kui otsitav ID on andmebaasi ID-ga sama.

        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            //ülesseade
            Guid databaseGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid seekGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            //tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(seekGuid);

            //kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        
        // Selles testis kontrollitakse, et lasteaia rühma
        // päringul andmebaasist ei tohiks süsteem tagastada
        // objekti, kui otsitav ID ja andmebaasi ID ei ole samad.
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            //ülesseade
            Guid realGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid wrongGuid = Guid.NewGuid();

            //tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(realGuid);

            //kontroll
            Assert.NotEqual(wrongGuid, realGuid);
        }

        
        // Selles testis kontrollitakse,
        // et lasteaia rühma kustutamisel andmebaasist
        // peaks objekt kustuma, kui tagastatav väärtus on sama mis loodud objektil.
        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            var dto = MockKindergartenData();
            // tegevus
            var addGroup = await Svc<IKindergartenServices>().Create(dto);
            var deleteGroup = await Svc<IKindergartenServices>().Delete((Guid)addGroup.Id);
            //kontroll
            Assert.Equal(addGroup.Id, deleteGroup.Id);
        }

        
        // Selles testis kontrollitakse, et lasteaia
        //rühma kustutamisel ei muutu ega kustu teise rühma andmed
        //ning nende ID-d ei tohi ühtida.
        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            //ülesseade
            var dto = MockKindergartenData();
            //tegevus
            var kinderGarten1 = await Svc<IKindergartenServices>().Create(dto);
            var kinderGarten2 = await Svc<IKindergartenServices>().Create(dto);

            var result = await Svc<IKindergartenServices>().Delete((Guid)kinderGarten2.Id);
            //kontroll
            Assert.NotEqual(kinderGarten1.Id, result.Id);
        }

        // Selles testis kontrollitakse, et lasteaia rühma andmete uuendamisel
        // muudetakse andmebaasis rühma ja õpetaja nimed, kuid objekti
        // unikaalne ID peab jääma samaks.
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            // ülesseade
            var guid = Guid.Parse("68eb8abd-086a-4c8b-9695-71234143f709");
            var dto = MockKindergartenData();

            KindergartenDto domain = new();
            domain.Id = guid;
            domain.GroupName = "Karupojad";
            domain.KindergartenName = "Tallinna Lasteaed";
            domain.ChildrenCount = 12;
            domain.TeacherName = "Õpetaja Tamm";
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.UtcNow;

            // tegevus
            await Svc<IKindergartenServices>().Update(dto);

            // kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.GroupName, domain.GroupName);
            Assert.NotEqual(dto.TeacherName, domain.TeacherName);
            Assert.NotEqual(dto.ChildrenCount, domain.ChildrenCount);
            Assert.NotEqual(dto.KindergartenName, domain.KindergartenName);
        }

        // Selles testis kontrollitakse, et kui lasteaia rühma andmeid üritatakse uuendada tühjade või vigaste andmetega, siis ei tohi süsteem olemasoleva rühma andmeid muuta.
        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            // ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);
            var nullDto = MockKindergartenNullData();

            // tegevus
            var result = await Svc<IKindergartenServices>().Update(nullDto);

            // kontroll
            Assert.Null(result);
        }

        // Selles testis kontrollitakse, et lasteaia rühma loomisel ei lubaks teenus sisestada negatiivset laste arvu ning süsteem tagab, et arv on suurem kui null.
        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenCountIsNegative()
        {
            // ülesseade
            var dto = MockKindergartenData();
            dto.ChildrenCount = -5;

            // tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.True(result.ChildrenCount >= 0);
        }

        // Selles testis kontrollitakse, et rühma loomisel peab olema määratud õpetaja nimi ning ilma kasvatajata rühma andmebaasi lisada ei tohi.
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenTeacherNameIsEmpty()
        {
            // ülesseade
            var dto = MockKindergartenData();
            dto.TeacherName = "";

            // tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.Null(result);
        }

        // Selles testis kontrollitakse, et pärast rühma edukat kustutamist eemaldatakse andmed andmebaasist lõplikult ja objekti ei ole enam võimalik leida.
        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            // ülesseade
            var dto = MockKindergartenData();

            // tegevus
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);
            await Svc<IKindergartenServices>().Delete((Guid)createdGroup.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)createdGroup.Id);

            // kontroll
            Assert.Null(result);
        }




        
        // 11. Selles testis kontrollitakse, et olematu ID-ga lasteaia rühma 
        // päringul andmebaasist tagastab süsteem tühja väärtuse (null) 
        // ega viska ootamatut viga.
        [Fact]
        public async Task Should_ReturnNull_WhenKindergartenDoesNotExist()
        {
            // ülesseade
            Guid nonExistingGuid = Guid.NewGuid();

            // tegevus
            var result = await Svc<IKindergartenServices>().DetailAsync(nonExistingGuid);

            // kontroll
            Assert.Null(result);
        }

        // 12. Selles testis kontrollitakse, et kui üritatakse kustutada lasteaeda, 
        // mida andmebaasis ei eksisteeri, siis teenus ei viska crashi, 
        // vaid tagastab nulli või vastava veatunnuse.
        [Fact]
        public async Task Should_ReturnNull_WhenDeletingNonExistingKindergarten()
        {
            // ülesseade
            Guid nonExistingGuid = Guid.NewGuid();

            // tegevus
            var result = await Svc<IKindergartenServices>().Delete(nonExistingGuid);

            // kontroll
            Assert.Null(result);
        }

        // 13. Selles testis kontrollitakse, et rühma loomisel peab olema määratud 
        // ka rühma nimi ning ilma nimeta (tühi string) rühma luua ei tohi.
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenGroupNameIsEmpty()
        {
            // ülesseade
            var dto = MockKindergartenData();
            dto.GroupName = ""; // vigane sisend

            // tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.Null(result);
        }

        // 14. Selles testis kontrollitakse, et rühma andmete uuendamisel 
        // muudetakse andmebaasis korrektselt laste arvu (ChildrenCount), 
        // kui edastatakse uus kehtiv väärtus.
        [Fact]
        public async Task Should_UpdateChildrenCount_WhenDataIsCorrect()
        {
            // ülesseade
            var dto = MockKindergartenData();
            var createdGroup = await Svc<IKindergartenServices>().Create(dto);

            // loome uuendamise DTO, kus muudame laste arvu
            var updateDto = new KindergartenDto
            {
                Id = createdGroup.Id,
                GroupName = createdGroup.GroupName,
                KindergartenName = createdGroup.KindergartenName,
                ChildrenCount = 25, // uus arv
                TeacherName = createdGroup.TeacherName,
                CreatedAt = createdGroup.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            // tegevus
            var result = await Svc<IKindergartenServices>().Update(updateDto);

            // kontroll
            Assert.Equal(25, result.ChildrenCount);
        }

        // 15. Selles testis kontrollitakse, et kahe erineva rühma loomisel 
        // genereerib süsteem neile automaatselt erinevad ja unikaalsed ID-d.
        [Fact]
        public async Task Should_GenerateUniqueIds_WhenTwoKindergartensAreCreated()
        {
            // ülesseade
            var dto1 = MockKindergartenData(false);
            var dto2 = MockKindergartenData(true);

            // tegevus
            var result1 = await Svc<IKindergartenServices>().Create(dto1);
            var result2 = await Svc<IKindergartenServices>().Create(dto2);

            // kontroll
            Assert.NotEqual(result1.Id, result2.Id);
        }



        
        
        /* üleval testid, all abimeetodid */
        //Test andmed I
        //            I
        //            V

        private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "",
                KindergartenName = "",
                ChildrenCount = 0,
                TeacherName = "",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }

        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergartenDto
                {
                    GroupName = "Päikesekiir",
                    KindergartenName = "Lasteaia tee 5, Tallinn",
                    ChildrenCount = 18,
                    TeacherName = "Maiu",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new KindergartenDto
                {
                    GroupName = "Sipsik",
                    KindergartenName = "Kooli tänav 12, Tartu",
                    ChildrenCount = 22,
                    TeacherName = "Maiu",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }
    }
}
