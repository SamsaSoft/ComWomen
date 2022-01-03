var selectedMediaTypes = { image: true, audio: true, video: true };
var activeFormLanguage = 'ru';

function clearLangInputs() {
    $('.lang_nav').removeClass("active");
    $('.lang_body').attr("hidden", "");
};

function initTabs (initHandler, clickHandler) {
    clearLangInputs();
    $(`#lang_nav_${activeFormLanguage}`).addClass("active");
    $(`#lang_body_${activeFormLanguage}`).removeAttr("hidden");
    if (initHandler) {
        initHandler();
    }
    $(`.lang_nav`).on("click", function () {
        clearLangInputs();
        $(this).addClass("active");
        activeFormLanguage = $(this).data("lang");
        $(`#lang_body_${activeFormLanguage}`).removeAttr("hidden");
        if (clickHandler) {
            clickHandler();
        }
    });
}

function createimagefromurl(url) {
    var htmlTag = $("<img>");
    htmlTag.attr("src", url);
    htmlTag.addClass("form-control img-fluid p-3");
    return htmlTag;
}

function createaudiofromurl(url) {
    var htmlTag = $("<audio>");
    htmlTag.attr("src", url);
    htmlTag.attr("controls", "");
    htmlTag.addClass("p-3");
    return htmlTag;
}

function createvideofromurl(url) {
    var htmlTag = $("<video>");
    htmlTag.attr("src", url);
    htmlTag.attr("controls", "");
    htmlTag.addClass("form-control img-fluid p-3");
    return htmlTag;
}

function createmediafromclass(cls, url) {
    if (cls == 'image') {
        return createimagefromurl(url);
    }
    if (cls == 'audio') {
        return createaudiofromurl(url);
    }
    if (cls == 'video') {
        return createvideofromurl(url);
    }
    return null;
}