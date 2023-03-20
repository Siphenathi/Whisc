$(document).ready(function () {
    $(document).on("scroll", onScroll);

    //smoothscroll
    $('.nav li a').on('click', function () {
        //e.preventDefault();
        $(document).off("scroll");

        $('a').each(function () {
            $(this).removeClass('active');
        })
        $(this).addClass('active');

        var target = this.hash,
            menu = target;
        $target = $(target);
        $('html, body').stop().animate({
            'scrollTop': $target.offset().top + 2
        }, 2000, 'swing', function () {
            window.location.hash = target;
            $(document).on("scroll", onScroll);
        });
    });
});

function onScroll(event) {
    var scrollPos = $(document).scrollTop();
    $('.navbar-collapse a').each(function () {
        var currLink = $(this);
        var refElement = $(currLink.attr("href"));
        if (refElement.position().top <= scrollPos && refElement.position().top + refElement.height() > scrollPos) {
            $('.navbar-collapse ul li a').removeClass("active");
            currLink.addClass("active");
        }
        else {
            currLink.removeClass("active");
        }
    });
}
//New small scroll button
//$(window).scroll(function ()
//{
//    if ($(this).scrollTop() > 100) {
//        $('.scrollup').fadeIn();
//    } else {
//        $('.scrollup').fadeOut();
//    }
//});
//$('.scrollup').click(function () {
//    $("html, body").animate({ scrollTop: 0 }, 1000);
//    return false;
//});
