function NewWindow(text) {
    win = window.open(text, '', 'top=0,left=0,width=400,height=350');
}
jQuery(document).ready(function ($) {
    //$(function(){

    //鎼滅储
    $(".search-reveal").click(function () {
        $(".row-search-mobile").slideToggle("400",
        function () { });
    });

    $('.placeholder').on('blur', function () {
        if ($(this).val() == "") {
            $(this).val("鎼滅储鈥︹€�");
        }
    });
    $('.placeholder').on('focus', function () {
        if ($(this).val() == '鎼滅储鈥︹€�') {
            $(this).val('');
        }
    });
    $('#feed_email').on('blur', function () {
        if ($(this).val() == "") {
            $(this).val("杈撳叆閭 璁㈤槄绗旇");
        }
    });
    $('#feed_email').on('focus', function () {
        if ($(this).val() == '杈撳叆閭 璁㈤槄绗旇') {
            $(this).val('');
        }
    });

    //浠ｇ爜楂樹寒
    if (!$("pre").hasClass("prettyprint")) {
        $("pre").addClass("prettyprint");
    }


    // 鍒楄〃
    color_flag = false; //閰嶈壊鏍囪
    prev_title_flag = false;
    next_title_flag = false;
    href = window.location.href;
    var total = $("#leftcolumn a").length;
    $("#leftcolumn").find("a").each(function (index, value) {
        if (next_title_flag) {
            return false; //缁撴潫寰幆
        }


        cur_href = $(this).attr("href");

        cur_obj = $(this);

        //if(href.match(cur_href) != null) {
        if (href.indexOf(cur_href) != -1) {
            //console.log('cur_href', cur_href);
            if (index == 0) {
                $(".previous-design-link").hide();
            }
            if (index == (total - 1)) {
                $(".next-design-link").hide();
            }


            if (cur_href.indexOf('/') == -1) { //绗簩閲嶅垽鏂�
                tmp_url = href.substring(0, href.lastIndexOf('/') + 1) + cur_href;

                if (href != tmp_url) return;
            }
            if (!color_flag) {
                $(this).css({ "background-color": "#96b97d", "font-weight": "bold", "color": "#fff" });
                color_flag = true;
            }
            prev_href = $(this).prev("a").attr("href");
            prev_title = $(this).prev("a").attr("title");
            if (!prev_title) prev_title = $(this).prev("a").text();
            next_href = $(this).next("a").attr("href");
            next_title = $(this).next("a").attr("title");
            if (!next_title) next_title = $(this).next("a").text();
            if (!prev_title_flag) {
                if (prev_title) {
                    $(".previous-design-link a").attr("href", prev_href);
                    $(".previous-design-link a").attr("title", prev_title);
                    $(".previous-design-link a").text(prev_title);
                } else {
                    if (typeof (prev_obj) != 'undefined') {
                        prev_href = prev_obj.attr("href");
                        prev_title = prev_obj.attr("title");
                        if (!prev_title) prev_title = prev_obj.text();
                        if (prev_title) {
                            $(".previous-design-link a").attr("href", prev_href);
                            $(".previous-design-link a").attr("title", prev_title);
                            $(".previous-design-link a").text(prev_title);
                        }
                    }

                }
                prev_title_flag = true;
            }
            if (next_title) {
                if ($(".next-design-link a").attr("href")) {
                    $(".next-design-link a").attr("href", next_href);
                    $(".next-design-link a").attr("title", next_title);
                    $(".next-design-link a").text(next_title);
                } else {
                    $(".next-design-link").html("<a href=\"" + next_href + "\" rel=\"next\" title=\"" + next_title + "\">" + next_title + "</a> &raquo;");
                }

                next_title_flag = true;

            }
            //return false; 
        } else {
            prev_obj = cur_obj;
            if (next_title_flag) {
                return false;
            } else {
                if (prev_title_flag) {
                    next_href = $(this).attr("href");
                    next_title = $(this).attr("title");
                    if (!next_title) next_title = $(this).text();
                    if (next_title) {
                        if ($(".next-design-link a").attr("href")) {
                            $(".next-design-link a").attr("href", next_href);
                            $(".next-design-link a").attr("title", next_title);
                            $(".next-design-link a").text(next_title);
                        } else {
                            $(".next-design-link").html("<a href=\"" + next_href + "\" rel=\"next\" title=\"" + next_title + "\">" + next_title + "</a> &raquo;");
                        }
                        next_title_flag = true;
                    }
                }
            }
        }
    });

    // 渚ф爮
    $(".sidebar-tree > ul > li").hover(function () {
        $(this).addClass("selected");
        $(this).children("a:eq(0)").addClass("h2-tit");
        $(this).children("ul").show();
    }, function () {
        $(this).removeClass("selected");
        $(this).children(".tit").removeClass("h2-tit");
        $(this).children("ul").hide();
    })
    // 鍏抽棴QQ缇�
    $(".qqinfo").hide();
    //$.getJSON("/try/qqinfo.php", function(data) {
    //	$("#qqid").text(data.qqid);
    //	$("#qqhref").attr("href", data.qqhref);
    //});
    // 棣栭〉瀵艰埅
    $("#index-nav li").click(function () {
        $(this).find("a").addClass("current");
        $(this).siblings().find("a").removeClass("current");
        id = $(this).find("a").attr("data-id");
        if (id == 'index') {

        }
        if (id == 'note') {

        } else if (id == 'tool') {

        } else if (id == 'quiz') {
            $("#tool").hide();
            $("#manual").hide();
            $("#" + id).show();
            $(".sub-navigation-articles").show();
        } else if (id == 'manual') {
            $("#tool").hide();
            $("#quiz").hide();
            $("#" + id).show();
            $(".sub-navigation-articles").show();
        } else {
            $("#tool").hide();
            $("#quiz").hide();
            $("#manual").hide();
        }
    });
    $("#cate0").click(function () {
        $(".codelist-desktop").show();
    })
    $(".design").click(function () {
        id = $(this).attr("id");
        $("." + id).show();
        $("." + id).siblings().hide();
    })
    //绉诲姩璁惧鐐瑰嚮閾炬帴	
    $('a').on('click touchend', function (e) {
        if (screen.availHeight == 548 && screen.availHeight == 320) {
            var el = $(this);
            var link = el.attr('href');
            window.location = link;
        }
    });

    $("#pull").click(function () {
        $(".left-column").slideToggle("400", function () { });
    })
    $(".qrcode").hover(function () {
        $("#bottom-qrcode").show();
    }, function () {
        $("#bottom-qrcode").hide();
    });
    $(window).scroll(function () {
        if ($(window).scrollTop() >= 100) {
            $(".go-top").fadeIn();
        } else {
            $(".go-top").fadeOut();
        }
    });


    $(".go-top").click(function (event) {
        $('html,body').animate({ scrollTop: 0 }, 100);
        return false;
    });
    $(window).resize(function () {
        var viewportWidth = $(window).width();
        if (window.location.href.indexOf("w3cnote") != -1) {
            //console.log('href', window.location.href);
        } else {
            if (viewportWidth > 768) {
                $(".left-column").show();
            }
        }

    });


});

// 鐧惧害鑷姩鎺ㄩ€�
(function () {
    var bp = document.createElement('script');
    bp.src = '//push.zhanzhang.baidu.com/push.js';
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(bp, s);
})();
$.browser.chrome = /chrom(e|ium)/.test(navigator.userAgent.toLowerCase());
if ($.browser.chrome) {
    console.log("%cRUNOOB.COM", "background: rgba(252,234,187,1);background: -moz-linear-gradient(left, rgba(252,234,187,1) 0%, rgba(175,250,77,1) 12%, rgba(0,247,49,1) 28%, rgba(0,210,247,1) 39%,rgba(0,189,247,1) 51%, rgba(133,108,217,1) 64%, rgba(177,0,247,1) 78%, rgba(247,0,189,1) 87%, rgba(245,22,52,1) 100%);background: -webkit-gradient(left top, right top, color-stop(0%, rgba(252,234,187,1)), color-stop(12%, rgba(175,250,77,1)), color-stop(28%, rgba(0,247,49,1)), color-stop(39%, rgba(0,210,247,1)), color-stop(51%, rgba(0,189,247,1)), color-stop(64%, rgba(133,108,217,1)), color-stop(78%, rgba(177,0,247,1)), color-stop(87%, rgba(247,0,189,1)), color-stop(100%, rgba(245,22,52,1)));background: -webkit-linear-gradient(left, rgba(252,234,187,1) 0%, rgba(175,250,77,1) 12%, rgba(0,247,49,1) 28%, rgba(0,210,247,1) 39%, rgba(0,189,247,1) 51%, rgba(133,108,217,1) 64%, rgba(177,0,247,1) 78%, rgba(247,0,189,1) 87%, rgba(245,22,52,1) 100%);background: -o-linear-gradient(left, rgba(252,234,187,1) 0%, rgba(175,250,77,1) 12%, rgba(0,247,49,1) 28%, rgba(0,210,247,1) 39%, rgba(0,189,247,1) 51%, rgba(133,108,217,1) 64%, rgba(177,0,247,1) 78%, rgba(247,0,189,1) 87%, rgba(245,22,52,1) 100%);background: -ms-linear-gradient(left, rgba(252,234,187,1) 0%, rgba(175,250,77,1) 12%, rgba(0,247,49,1) 28%, rgba(0,210,247,1) 39%, rgba(0,189,247,1) 51%, rgba(133,108,217,1) 64%, rgba(177,0,247,1) 78%, rgba(247,0,189,1) 87%, rgba(245,22,52,1) 100%);background: linear-gradient(to right, rgba(252,234,187,1) 0%, rgba(175,250,77,1) 12%, rgba(0,247,49,1) 28%, rgba(0,210,247,1) 39%, rgba(0,189,247,1) 51%, rgba(133,108,217,1) 64%, rgba(177,0,247,1) 78%, rgba(247,0,189,1) 87%, rgba(245,22,52,1) 100%);filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fceabb', endColorstr='#f51634', GradientType=1 );font-size:5em");
}