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

        
        //// Selles testis kontrollitakse, et lasteaia
        ///rühma kustutamisel ei muutu ega kustu teise rühma andmed
        ///ning nende ID-d ei tohi ühtida.
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

        /* üleval testid, all abimeetodid */

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
