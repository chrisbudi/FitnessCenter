var PersonScript = function () {
    return {

        //Script untuk Link Auth
        getAge: function getAge(dateString) {
            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();

            var dob = new Date(
                dateString.substring(6, 10),
                dateString.substring(0, 2) - 1,
                dateString.substring(3, 5)
            );

            var yearDob = dob.getYear();
            var monthDob = dob.getMonth();
            var dateDob = dob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }
            var dateAge;
            if (dateNow >= dateDob) {
                dateAge = dateNow - dateDob;
            } else {
                monthAge--;
                dateAge = 31 + dateNow - dateDob;
                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };

            if (age.years > 1) yearString = " th";
            else yearString = " th";
            if (age.months > 1) monthString = " bl";
            else monthString = " bl";
            if (age.days > 1) dayString = " hr";
            else dayString = " hr";


            if ((age.years > 0) && (age.months > 0) && (age.days > 0))
                ageString = age.years + yearString + ", " + age.months + monthString + ", " + age.days + dayString + " ";
            else if ((age.years == 0) && (age.months == 0) && (age.days > 0))
                ageString = "Only " + age.days + dayString + " ";
            else if ((age.years > 0) && (age.months == 0) && (age.days == 0))
                ageString = age.years + yearString + ". Happy Birthday!!";
            else if ((age.years > 0) && (age.months > 0) && (age.days == 0))
                ageString = age.years + yearString + " " + age.months + monthString + " ";
            else if ((age.years == 0) && (age.months > 0) && (age.days > 0))
                ageString = age.months + monthString + " " + age.days + dayString + " ";
            else if ((age.years > 0) && (age.months == 0) && (age.days > 0))
                ageString = age.years + yearString + " " + age.days + dayString + " ";
            else if ((age.years == 0) && (age.months > 0) && (age.days == 0))
                ageString = age.months + monthString + " ";
            else ageString = "Oops! Could not calculate age!";

            return ageString;
        }
    }
}();
// End  Script untuk Link Auth