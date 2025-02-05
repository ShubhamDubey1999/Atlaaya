$(document).ready(function () {
    $(".project").fadeIn(500);

    $(".projBtns button").click(function (e) {
        e.stopPropagation();

        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
            $(".project").fadeIn(500);
        } else {
            $(".projBtns button").removeClass("active");
            $(this).addClass("active");

            var filterCategory = $(this).text();

            $(".project").fadeOut(300);

            setTimeout(function () {
                if (filterCategory === "Commercial") {
                    $(".project[data-category='Commercial']").fadeIn(500);
                } else if (filterCategory === "Hospitality") {
                    $(".project[data-category='Hospitality']").fadeIn(500);
                } else if (filterCategory === "Residential") {
                    $(".project[data-category='Residential']").fadeIn(500);
                }
            }, 300);
        }
    });

    $(document).click(function (e) {
        if (!$(e.target).closest(".projBtns").length) {
            $(".projBtns button").removeClass("active");
            $(".project").fadeIn(500);
        }
    });
});