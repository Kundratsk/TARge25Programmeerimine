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
