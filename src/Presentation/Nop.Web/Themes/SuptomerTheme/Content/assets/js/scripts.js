(function ($) {
  "use strict";

  /* Preloader */
  $(window).on("load", function () {
    var preloaderFadeOutTime = 500;
    function hidePreloader() {
      var preloader = $(".spinner-wrapper");
      setTimeout(function () {
        preloader.fadeOut(preloaderFadeOutTime);
      }, 500);
    }
    hidePreloader();
  });

  /* Navbar Scripts */
  // jQuery to collapse the navbar on scroll
  $(window).on("scroll load", function () {
    if ($(".navbar").offset().top > 60) {
      $(".fixed-top").addClass("top-nav-collapse");
    } else {
      $(".fixed-top").removeClass("top-nav-collapse");
    }
  });

  // jQuery for page scrolling feature - requires jQuery Easing plugin
  $(function () {
    $(document).on("click", "a.page-scroll", function (event) {
      var $anchor = $(this);
      $("html, body")
        .stop()
        .animate(
          {
            scrollTop: $($anchor.attr("href")).offset().top,
          },
          600,
          "easeInOutExpo"
        );
      event.preventDefault();
    });
  });

  // closes the responsive menu on menu item click
  $(".navbar-nav li a").on("click", function (event) {
    if (!$(this).parent().hasClass("dropdown"))
      $(".navbar-collapse").collapse("hide");
  });

  /* Image Slider - Swiper */
  var imageSlider = new Swiper(".image-slider", {
    autoplay: {
      delay: 2000,
      disableOnInteraction: false,
    },
    loop: true,
    spaceBetween: 30,
    slidesPerView: 5,
    breakpoints: {
      // when window is <= 580px
      580: {
        slidesPerView: 1,
        spaceBetween: 10,
      },
      // when window is <= 768px
      768: {
        slidesPerView: 2,
        spaceBetween: 20,
      },
      // when window is <= 992px
      992: {
        slidesPerView: 3,
        spaceBetween: 20,
      },
      // when window is <= 1200px
      1200: {
        slidesPerView: 4,
        spaceBetween: 20,
      },
    },
  });

  /* Card Slider - Swiper */
  var cardSlider = new Swiper(".card-slider", {
    autoplay: {
      delay: 4000,
      disableOnInteraction: false,
    },
    loop: true,
    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },
  });

  /* Video Lightbox - Magnific Popup */
  $(".popup-youtube, .popup-vimeo").magnificPopup({
    disableOn: 700,
    type: "iframe",
    mainClass: "mfp-fade",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: false,
    iframe: {
      patterns: {
        youtube: {
          index: "youtube.com/",
          id: function (url) {
            var m = url.match(/[\\?\\&]v=([^\\?\\&]+)/);
            if (!m || !m[1]) return null;
            return m[1];
          },
          src: "https://www.youtube.com/embed/%id%?autoplay=1",
        },
        vimeo: {
          index: "vimeo.com/",
          id: function (url) {
            var m = url.match(
              /(https?:\/\/)?(www.)?(player.)?vimeo.com\/([a-z]*\/)*([0-9]{6,11})[?]?.*/
            );
            if (!m || !m[5]) return null;
            return m[5];
          },
          src: "https://player.vimeo.com/video/%id%?autoplay=1",
        },
      },
    },
  });

  /* Lightbox - Magnific Popup */
  $(".popup-with-move-anim").magnificPopup({
    type: "inline",
    fixedContentPos: false /* keep it false to avoid html tag shift with margin-right: 17px */,
    fixedBgPos: true,
    overflowY: "auto",
    closeBtnInside: true,
    preloader: false,
    midClick: true,
    removalDelay: 300,
    mainClass: "my-mfp-slide-bottom",
  });

  /* Move Form Fields Label When User Types */
  // for input and textarea fields
  $("input, textarea").keyup(function () {
    if ($(this).val() != "") {
      $(this).addClass("notEmpty");
    } else {
      $(this).removeClass("notEmpty");
    }
  });

  /* Request Form */
  $("#requestForm")
    .validator()
    .on("submit", function (event) {
      if (event.isDefaultPrevented()) {
        // handle the invalid form...
        rformError();
        rsubmitMSG(false, "Please fill all fields!");
      } else {
        // everything looks good!
        event.preventDefault();
        rsubmitForm();
      }
    });

  function rsubmitForm() {
    // initiate variables with form content
    var name = $("#rname").val();
    var email = $("#remail").val();
    var phone = $("#rphone").val();
    var select = $("#rselect").val();
    var terms = $("#rterms").val();

    $.ajax({
      type: "POST",
      url: "php/requestform-process.php",
      data:
        "name=" +
        name +
        "&email=" +
        email +
        "&phone=" +
        phone +
        "&select=" +
        select +
        "&terms=" +
        terms,
      success: function (text) {
        if (text == "success") {
          rformSuccess();
        } else {
          rformError();
          rsubmitMSG(false, text);
        }
      },
    });
  }

  function rformSuccess() {
    $("#requestForm")[0].reset();
    rsubmitMSG(true, "Request Submitted!");
    $("input").removeClass("notEmpty"); // resets the field label after submission
  }

  function rformError() {
    $("#requestForm")
      .removeClass()
      .addClass("shake animated")
      .one(
        "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
        function () {
          $(this).removeClass();
        }
      );
  }

  function rsubmitMSG(valid, msg) {
    if (valid) {
      var msgClasses = "h3 text-center tada animated";
    } else {
      var msgClasses = "h3 text-center";
    }
    $("#rmsgSubmit").removeClass().addClass(msgClasses).text(msg);
  }

  /* Contact Form */
  $("#contactForm")
    .validator()
    .on("submit", function (event) {
      if (event.isDefaultPrevented()) {
        // handle the invalid form...
        cformError();
        csubmitMSG(false, "Please fill all fields!");
      } else {
        // everything looks good!
        event.preventDefault();
        csubmitForm();
      }
    });

  function csubmitForm() {
    // initiate variables with form content
    var name = $("#cname").val();
    var email = $("#cemail").val();
    var message = $("#cmessage").val();
    var terms = $("#cterms").val();
    $.ajax({
      type: "POST",
      url: "php/contactform-process.php",
      data:
        "name=" +
        name +
        "&email=" +
        email +
        "&message=" +
        message +
        "&terms=" +
        terms,
      success: function (text) {
        if (text == "success") {
          cformSuccess();
        } else {
          cformError();
          csubmitMSG(false, text);
        }
      },
    });
  }

  function cformSuccess() {
    $("#contactForm")[0].reset();
    csubmitMSG(true, "Message Submitted!");
    $("input").removeClass("notEmpty"); // resets the field label after submission
    $("textarea").removeClass("notEmpty"); // resets the field label after submission
  }

  function cformError() {
    $("#contactForm")
      .removeClass()
      .addClass("shake animated")
      .one(
        "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
        function () {
          $(this).removeClass();
        }
      );
  }

  function csubmitMSG(valid, msg) {
    if (valid) {
      var msgClasses = "h3 text-center tada animated";
    } else {
      var msgClasses = "h3 text-center";
    }
    $("#cmsgSubmit").removeClass().addClass(msgClasses).text(msg);
  }

  /* Privacy Form */
  $("#privacyForm")
    .validator()
    .on("submit", function (event) {
      if (event.isDefaultPrevented()) {
        // handle the invalid form...
        pformError();
        psubmitMSG(false, "Please fill all fields!");
      } else {
        // everything looks good!
        event.preventDefault();
        psubmitForm();
      }
    });

  function psubmitForm() {
    // initiate variables with form content
    var name = $("#pname").val();
    var email = $("#pemail").val();
    var select = $("#pselect").val();
    var terms = $("#pterms").val();

    $.ajax({
      type: "POST",
      url: "php/privacyform-process.php",
      data:
        "name=" +
        name +
        "&email=" +
        email +
        "&select=" +
        select +
        "&terms=" +
        terms,
      success: function (text) {
        if (text == "success") {
          pformSuccess();
        } else {
          pformError();
          psubmitMSG(false, text);
        }
      },
    });
  }

  function pformSuccess() {
    $("#privacyForm")[0].reset();
    psubmitMSG(true, "Request Submitted!");
    $("input").removeClass("notEmpty"); // resets the field label after submission
  }

  function pformError() {
    $("#privacyForm")
      .removeClass()
      .addClass("shake animated")
      .one(
        "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend",
        function () {
          $(this).removeClass();
        }
      );
  }

  function psubmitMSG(valid, msg) {
    if (valid) {
      var msgClasses = "h3 text-center tada animated";
    } else {
      var msgClasses = "h3 text-center";
    }
    $("#pmsgSubmit").removeClass().addClass(msgClasses).text(msg);
  }

  /* Back To Top Button */
  // create the back to top button
  $("body").prepend(
    '<a href="body" class="back-to-top page-scroll">Back to Top</a>'
  );
  var amountScrolled = 700;
  $(window).scroll(function () {
    if ($(window).scrollTop() > amountScrolled) {
      $("a.back-to-top").fadeIn("500");
    } else {
      $("a.back-to-top").fadeOut("500");
    }
  });

  /* Removes Long Focus On Buttons */
  $(".button, a, button").mouseup(function () {
    $(this).blur();
  });

  //show hide password
  $(".form-group #show_hide_cpassword .input-group-text").on(
    "click",
    function (event) {
      event.preventDefault();
      if ($("#show_hide_cpassword input").attr("type") == "text") {
        $("#show_hide_cpassword input").attr("type", "password");
        $("#show_hide_cpassword i").addClass("fa-eye-slash");
        $("#show_hide_cpassword i").removeClass("fa-eye");
      } else if ($("#show_hide_cpassword input").attr("type") == "password") {
        $("#show_hide_cpassword input").attr("type", "text");
        $("#show_hide_cpassword i").removeClass("fa-eye-slash");
        $("#show_hide_cpassword i").addClass("fa-eye");
      }
    }
  );
  $("#show_hide_password .input-group-text").on("click", function (event) {
    event.preventDefault();
    if ($("#show_hide_password input").attr("type") == "text") {
      $("#show_hide_password input").attr("type", "password");
      $("#show_hide_password i").addClass("fa-eye-slash");
      $("#show_hide_password i").removeClass("fa-eye");
    } else if ($("#show_hide_password input").attr("type") == "password") {
      $("#show_hide_password input").attr("type", "text");
      $("#show_hide_password i").removeClass("fa-eye-slash");
      $("#show_hide_password i").addClass("fa-eye");
    }
  });

  $(".profilepassword .input-group-text").on("click", function (event) {
    event.preventDefault();
    var inputField = $(this).parent().find(".password");
    var eyeIcon = $(this).find("i");

    if (inputField.attr("type") === "password") {
      inputField.attr("type", "text");
      eyeIcon.removeClass("fa-eye");
      eyeIcon.addClass("fa-eye-slash");
    } else if (inputField.attr("type") === "text") {
      inputField.attr("type", "password");
      eyeIcon.removeClass("fa-eye-slash");
      eyeIcon.addClass("fa-eye");
    }
  });
  //email service
  //check max input char
  // Attach an event listener to all input fields
  $("input[maxlength]").on("input", function () {
    var maxLength = parseInt($(this).attr("maxlength"));
    var inputLength = $(this).val().length;
    var errorDiv = $("#errorDiv");

    // Check if input length exceeds the maximum length
    if (inputLength >= maxLength) {
      errorDiv
        .removeClass("d-none")
        .text("Maximum number of characters is " + maxLength); // Show error message
    } else {
      errorDiv.addClass("d-none"); // Hide error message
    }
  });
  //check time 'from' is not greater than 'to'/ time
  // $('input[type="time"]').on("input", function () {
  //  
  //   var startTime = $(".startTime").val();
  //   var endTime = $(".endTime").val();
  //   var errorDiv = $("#errorDiv");
  //   // Compare start time and end time
  //   if (endTime !== '' && startTime >= endTime) {
  //    
  //     errorDiv.removeClass("d-none");// Show error message
  //       $("#saveBranchBtn").prop("disabled", true);
  //       $("#btnnext_branches").prop("disabled", true);
  //   } else {
  //     $("#saveBranchBtn").prop("disabled", false);
  //     $("#btnnext_branches").prop("disabled", false);

  //     // If everything is okay, submit the form
  //     errorDiv.addClass("d-none"); // Hide error message
  //     //$('#myForm').submit();
  //   }
  // });

  $('input[type="time"].startTime').on("input", function () {
    var errorDiv = $(".errorDiv");

    // Reset disabled state for buttons
    $("#saveBranchBtn, #btnnext_branches").prop("disabled", false);
   
    // Loop through each pair of start and end times
    $('input[type="time"].endTime').each(function (index) {
      $('input[type="time"].endTime').on("input", function () {
        var startTime = $(".startTime").eq(index).val();
        var endTime = $(this).val();
        var diff = ( new Date("1970-1-1 " + startTime) - new Date("1970-1-1 " + endTime) ) / 1000 / 60 / 60; 
        // Compare start time and end time
        debugger;
        if (endTime !== "" && diff > 0) {
          errorDiv.eq(index).removeClass("d-none"); // Show error message
          $("#saveBranchBtn, #btnnext_branches").prop("disabled", true);
          // return false; // Exit the loop early if an error is found
        } else {
         
          $("#saveBranchBtn, #btnnext_branches").prop("disabled", false);
          // If everything is okay, submit the form
          errorDiv.eq(index).addClass("d-none"); // Hide error message
        }
      });
    });

    // If everything is okay, hide error message
    //   errorDiv.addClass("d-none");
  });
  $('input[type="time"].endTime').on("input", function () {
    var errorDiv = $(".errorDiv");

    // Reset disabled state for buttons
    $("#saveBranchBtn, #btnnext_branches").prop("disabled", false);
   
    // Loop through each pair of start and end times
    $('input[type="time"].startTime').each(function (index) {
      $('input[type="time"].startTime').on("input", function () {
        var endTime = $(".endTime").eq(index).val();
        var startTime = $(this).val();
        var diff = ( new Date("1970-1-1 " + startTime) - new Date("1970-1-1 " + endTime) ) / 1000 / 60 / 60; 
        // Compare start time and end time
        debugger;
        if (endTime !== "" && diff > 0) {
          errorDiv.eq(index).removeClass("d-none"); // Show error message
          $("#saveBranchBtn, #btnnext_branches").prop("disabled", true);
          // return false; // Exit the loop early if an error is found
        } else {
         
          $("#saveBranchBtn, #btnnext_branches").prop("disabled", false);
          // If everything is okay, submit the form
          errorDiv.eq(index).addClass("d-none"); // Hide error message
        }
      });
    });

    // If everything is okay, hide error message
    //   errorDiv.addClass("d-none");
  });
  $(function () {
    //$(".chevron-down").
    $("div[data-toggle]").click(function () {
      $(this).children("span").toggleClass("fa-chevron-down fa-chevron-up");
    });
  });

  $(document).ready(function () {
    function showLoading() {
      $("#loading").show();
    }

    function hideLoading() {
      $("#loading").hide();
    }

    $("#contact-form").submit(function (e) {
      e.preventDefault();

      var name = $("#name").val();
      var email = $("#email").val();
      var phone = $("#phone").val();
      var message = $("#message").val();
      // Initialize EmailJS with your user ID
      emailjs.init("ZxmMKZWhq2xh6Ypdo");

      // Set your email template parameters
      var templateParams = {
        name: name,
        email: email,
        phone: phone,
        message_html: message,
      };
      showLoading();

      // Send email using EmailJS

      emailjs
        .send("service_hq86vvd", "template_tcn5oro", templateParams)
        .then(
          function (response) {
            $("#success-message").text("Email sent successfully!");
            $("#contact-form")[0].reset();
            // alert('sent')
          },
          function (error) {
            $("#error-message").text(
              "An error occurred while sending the email."
            );
            //  alert('error');
          }
        )
        .finally(function () {
          hideLoading();
        });
    });
  });

  //toggle collaps accordion
  // $(function(){
  //   //$(".chevron-down").
  //   $("div[data-toggle=collapse]").click(function(){
  //       $(this).children('span').toggleClass("fa-chevron-down fa-chevron-up");
  //   });
  // })
})(jQuery);
