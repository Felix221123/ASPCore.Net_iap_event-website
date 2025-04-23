// Write your JavaScript code.
$(document).ready(function () {
    console.log("jQuery is working!");
    console.log("Event details page is working!");

    // Cache elements
    const $hamburgerMenu = $(".hamburger-menu");
    const $nav = $("nav");
    const $closeBtn = $("#close-tbn");
    const $navLinks = $("#my-list-of-headers li a");
    const $avatar = $("#avatar");
    const $themeButton = $(".btn-reserve");
    const $increaseFontBtn = $("#increase-font");
    const $decreaseFontBtn = $("#decrease-font");
    const $resetFontBtn = $("#reset-font");
    const $eventContainer = $(".eventsContainer");
    const $sortButton = $(".sortContainer");
    const $searchTitle = $("#eventtitle");
    const $searchPlace = $("#place");
    const $preloader = $(".preloader-container");
    const $searchResultsContainer = $(".searchResultsContainer");

    // Animation class or properties
    const animationSettings = {
        duration: 500, // Animation duration (ms)
        easing: "swing", // Easing effect
    };

    // Open menu when hamburger is clicked
    $hamburgerMenu.on("click", function () {
        $nav.slideDown(animationSettings); // Animates the appearance of the nav
    });

    // Close menu when close button is clicked
    $closeBtn.on("click", function () {
        $nav.slideUp(animationSettings); // Animates the disappearance of the nav
    });

    const siteLinks = [$navLinks, $avatar];

    // Close menu when any nav link is clicked
    siteLinks.forEach(link => {
        link.on("click", function (event) {
            if (window.innerWidth < 768) {
                event.preventDefault(); // Prevent default link behavior
                const targetHref = $(this).attr("href"); // Get the link's href

                // Slide up the menu
                $nav.slideUp(animationSettings.duration, animationSettings.easing, function () {
                    // Navigate to the link after the animation completes
                    window.location.href = targetHref;
                });
            } 
        });
    });

    // Close menu when clicking outside the nav
    $(document).on("click", function (event) {
        if (
            window.innerWidth < 768 &&
            !$nav.is(event.target) && // If the click is NOT on the nav itself
            $nav.has(event.target).length === 0 && // If the click is NOT inside the nav
            !$hamburgerMenu.is(event.target) // If the click is NOT on the hamburger menu
        ) {
            $nav.slideUp(animationSettings);
        }
    });


    // here are some of the new stuffs

    $('.ticket-title').on('click', function (e) {
        e.preventDefault();
        var ticketId = $(this).data('ticket-id');
        $('#ticket-popup-' + ticketId).fadeIn();
    });

    $('.close-popup').on('click', function () {
        var ticketId = $(this).data('ticket-id');
        $('#ticket-popup-' + ticketId).fadeOut();
    });



    // Show Add Form
    $('#showAddEventBtn').click(function () {
        $('#eventFormContainer').slideDown();
        // clearEventForm();
    });


     // Event listener for clicking on any event title
    $('.event-title').click(function () {
        // Retrieve event details from data-* attributes
        var eventId = $(this).data('event-id');
        var eventName = $(this).data('name');
        var eventImages = $(this).data('images');
        var eventDescription = $(this).data('description');
        var eventType = $(this).data('type');
        var eventDay = $(this).data('day');
        var eventMonth = $(this).data('month');
        var eventYear = $(this).data('year');
        var eventTime = $(this).data('time');
        var venueName = $(this).data('venue-name');
        var venueAddress = $(this).data('venue-address');
        var organizerName = $(this).data('organizer-name');
        var organizerContact = $(this).data('organizer-contact');
        var followLink = $(this).data('follow-link');
        var ticketPrice = $(this).data('ticket-price');
        var currency = $(this).data('currency');
        var eventLink = $(this).data('event-link');

        console.log("Event ID:", eventId);
        console.log("Event Name:", eventName);
        console.log("Event Images:", eventImages);
        console.log("Event Description:", eventDescription);
        console.log("Event Type:", eventType);
        console.log("Event Day:", eventDay);
        console.log("Event Month:", eventMonth);
        console.log("Event Year:", eventYear);
        console.log("Event Time:", eventTime);
        console.log("Venue Name:", venueName);
        console.log("Venue Address:", venueAddress);
        console.log("Organizer Name:", organizerName);
        console.log("Organizer Contact:", organizerContact);
        console.log("Follow Link:", followLink);
        console.log("Ticket Price:", ticketPrice);
        console.log("Currency:", currency);
        console.log("Event Link:", eventLink);

        console.log("Event Images:", eventImages); // Log to see the object

        // Convert eventImages to a string (JSON format)
        var eventImagesString = JSON.stringify(eventImages);
        console.log("Event Images as String:", eventImagesString);
        
        // Populate the form fields using jQuery (instead of querySelector)
        $("input[name='UpdateEvent.EventID']").val(eventId);
        $("input[name='UpdateEvent.Name']").val(eventName);
        $("textarea[name='UpdateEvent.Images']").val(eventImagesString);
        $("textarea[name='UpdateEvent.Description']").val(eventDescription);
        $("input[name='UpdateEvent.Type']").val(eventType);
        $("input[name='UpdateEvent.Day']").val(eventDay);
        $("input[name='UpdateEvent.Month']").val(eventMonth);
        $("input[name='UpdateEvent.Year']").val(eventYear);
        $("input[name='UpdateEvent.Time']").val(eventTime);
        $("input[name='UpdateEvent.VenueName']").val(venueName);
        $("textarea[name='UpdateEvent.VenueAddress']").val(venueAddress);
        $("input[name='UpdateEvent.OrganizerName']").val(organizerName);
        $("input[name='UpdateEvent.OrganizerContact']").val(organizerContact);
        $("input[name='UpdateEvent.FollowLink']").val(followLink);
        $("input[name='UpdateEvent.TicketPrice']").val(ticketPrice);
        $("input[name='UpdateEvent.Currency']").val(currency);
        $("input[name='UpdateEvent.EventLink']").val(eventLink);
    
        // Show the update form
        $('#UpdateEventFormContainer').slideDown();
    });
    

    // Cancel Form
    $('#cancelEventBtn').click(function () {
        $('#eventFormContainer').slideUp();
        // clearEventForm();
    });
    $('#cancelUpdateEventBtn').click(function () {
        $('#UpdateEventFormContainer').slideUp();
        // clearEventForm();
    });


    $('#closePopup').click(function () {
        $('#success-popup').fadeOut();
    });

    $('.user-email').on('click', function () {
        const id = $(this).data('user-id');
        const firstName = $(this).data('firstname');
        const lastName = $(this).data('lastname');
        const email = $(this).data('email');
        const password = $(this).data('password');
    
        // Fill the form
        $('#UserID').val(id);
        $('#UserFirstName').val(firstName);
        $('#UserLastName').val(lastName);
        $('#UserEmail').val(email);
        $('#UserPassword').val(password);
    
        $('#userUpdateFormContainer').slideDown();
    });
    
    $('#cancelUserBtn').on('click', function () {
        $('#userUpdateFormContainer').slideUp();
    });   

    $('.add-event').on('click', function () {
       console.log('im pressed')
    });   

    $('.message-email').on('click', function() {
        // Retrieve data attributes from the clicked element
        var fullName = $(this).data('fullname');
        var email = $(this).data('email');
        var phone = $(this).data('phone');
        var message = $(this).data('message');
        var sentAt = $(this).data('sentat');

        // Populate modal fields
        $('#modalFullName').val(fullName);
        $('#modalEmail').val(email);
        $('#modalPhoneNumber').val(phone);
        $('#modalSentAt').val(sentAt);
        $('#modalMessage').val(message);

        // Show the modal
        var messageModal = new bootstrap.Modal(document.getElementById('messageModal'));
        messageModal.show();
    });
    








    // Function to apply the theme based on mode
    const applyTheme = (mode) => {
        if (document.body.id === "profile-page") return;
        if (document.body.id === "adminDashboard") return;
        
        if (mode === "dark") {
            $("body").css({
                "background-color": "#141D2F",
            });
            $(".dark-mode-clr").css("color", "#FFFFFF");
            $(".darkmode-bg-clr").css("background-color", "transparent");

            $themeButton.text("Light ").append('<img src="/assets/svg/sun.svg" alt="sun icon" id="light-icon">');
            localStorage.setItem("theme", "dark");

        } else {
            $("body").css({
                "background-color": "white",
            });
            $(".dark-mode-clr").css("color", "");
            $themeButton.text("Dark ").append('<img src="/assets/svg/moon.svg" alt="moon icon" id="dark-icon">');
            localStorage.setItem("theme", "light");
        }

        // Reapply theme styles to dynamically created buttons
        $(".filter-btn.dark-mode-clr, .more-options.dark-mode-clr option").each(function () {
            if (mode === "dark") {
                $(this).css("color", "#FFFFFF");
                localStorage.setItem("theme", "dark");
            } else {
                $(this).css("color", "");
                localStorage.setItem("theme", "light");
            }
        });
    };

    // Load theme from localStorage on page load
    const savedTheme = localStorage.getItem("theme") || "light";
    applyTheme(savedTheme);

    // Theme toggle event
    $themeButton.on("click", function () {
        const currentTheme = localStorage.getItem("theme");
        const newTheme = currentTheme === "dark" ? "light" : "dark";
        applyTheme(newTheme);
    });


    // Classes to target for font size adjustments
    const targetClass = ".dark-mode-clr";
    const fontIncrement = 0.025; // 0.3rem increment
    // let defaultFontSize;

    // Function to save current font sizes to localStorage
    const saveFontSizesToLocalStorage = () => {
        const fontSizes = {};
        $(targetClass).each(function (index) {
            const currentFontSize = parseFloat($(this).css("font-size"));
            fontSizes[index] = currentFontSize;
        });
        localStorage.setItem("fontSizes", JSON.stringify(fontSizes));
    };

    // Function to apply saved font sizes from localStorage
    const applySavedFontSizes = () => {
        const savedFontSizes = JSON.parse(localStorage.getItem("fontSizes"));
        if (savedFontSizes) {
            $(targetClass).each(function (index) {
                if (savedFontSizes[index]) {
                    $(this).css("font-size", savedFontSizes[index] + "px");
                }
            });
        }
    };

    // Initialize default font sizes for each element
    $(targetClass).each(function () {
        const currentFontSize = parseFloat($(this).css("font-size")); // Get current font size in px
        $(this).attr("data-default-font-size", currentFontSize); // Store the default font size as a data attribute
    });

    // Apply saved font sizes on page load
    applySavedFontSizes();

    // Increase font size
    $increaseFontBtn.on("click", function () {
        $(targetClass).each(function () {
            const currentFontSize = parseFloat($(this).css("font-size")); // Get current font size
            const newFontSize = currentFontSize + fontIncrement * 16; // Increase by 0.025rem (converted to px)
            $(this).css("font-size", newFontSize + "px");
        });
        saveFontSizesToLocalStorage(); // Save the updated font sizes
    });

    // Decrease font size
    $decreaseFontBtn.on("click", function () {
        $(targetClass).each(function () {
            const currentFontSize = parseFloat($(this).css("font-size")); // Get current font size
            const newFontSize = currentFontSize - fontIncrement * 16; // Decrease by 0.025rem (converted to px)
            if (newFontSize > 0) { // Prevent font size from going negative
                $(this).css("font-size", newFontSize + "px");
            }
        });
        saveFontSizesToLocalStorage(); // Save the updated font sizes
    });

    // Reset font size
    $resetFontBtn.on("click", function () {
        $(targetClass).each(function () {
            const defaultFontSize = $(this).data("default-font-size"); // Retrieve the default font size from the data attribute
            $(this).css("font-size", defaultFontSize + "px"); // Reset to original font size
        });
        localStorage.removeItem("fontSizes"); // Clear saved font sizes from localStorage
    });

    // Validate Full Name
    $("#Contact_FullName").on("input", function () {
        const fullName = $(this).val().trim();
        const nameRegex = /^[a-zA-Z\s]+$/;
        if (fullName === "" | !nameRegex.test(fullName)) {
            $(this).css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        } else {
            $(this).css({ "border-bottom": "2px solid green", "transition": "border-bottom 0.3s ease" });
        }
    });

    // Validate Email Address
    $("#Contact_Email").on("input", function () {
        const email = $(this).val().trim();
        const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
        if (email === "" || !emailRegex.test(email)) {
            $(this).css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        } else {
            $(this).css({ "border-bottom": "2px solid green", "transition": "border-bottom 0.3s ease" });
        }
    });

    // Phone Number (Optional)
    $("#Contact_PhoneNumber").on("input", function () {
        const phoneNumber = $(this).val().trim();
        if (phoneNumber && isNaN(phoneNumber)) {
            $(this).css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        } else {
            $(this).css({ "border-bottom": "2px solid green", "transition": "border-bottom 0.3s ease" });
        }
    });

    // Message (Optional)
    $("#Contact_Message").on("input", function () {
        const message = $(this).val().trim();
        if (message === "") {
            $(this).css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        } else {
            $(this).css({ "border-bottom": "2px solid green", "transition": "border-bottom 0.3s ease" });
        }
    });

    // On Submit, Validate All Fields
    $("form.contactForm").on("submit", function (e) {

        let isValid = true;

        // Validate Full Name
        const fullName = $("#Contact_FullName").val().trim();
        if (fullName === "") {
            isValid = false;
            $("#Contact_FullName").css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        }

        // Validate Email Address
        const email = $("#Contact_Email").val().trim();
        const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
        if (email === "" || !emailRegex.test(email)) {
            isValid = false;
            $("#Contact_Email").css({ "border-bottom": "2px solid red", "transition": "border-bottom 0.3s ease" });
        }

        // Optional fields already handled during input events

        if (isValid) {
            console.log("Form submitted successfully!");
            // Uncomment the line below to allow actual form submission
            // this.submit();
        } else {
            alert("Please fill out the required fields before submitting.");
            e.preventDefault(); // Prevent form submission if validation fails
        }
    });


    // Function to create filter buttons
    function createFilterButtons(events) {
        const categories = [...new Set(events.map(event => event.type))]; // Unique categories
        const $filterContainer = $(".filterContainer");

        $filterContainer.empty();

        // Add "All" button
        $filterContainer.append('<button class="filter-btn cursor-pointer dark-mode-clr activeCategory" data-category="all">All</button>');

        // Add first 3 categories
        categories.slice(0, 5).forEach(category => {
            $filterContainer.append(`<button class="filter-btn cursor-pointer dark-mode-clr" data-category="${category}">${category}</button>`);
        });

        // Add "More" dropdown button
        if (categories.length > 5) {
            const dropdown = `
                <div class="more-categories">
                    <select class="more-options cursor-pointer">
                        <option value="more" disabled selected class="dark-mode-clr">More...</option>
                        ${categories.slice(5).map(cat => `<option value="${cat}" class="dark-mode-clr options-value">${cat}</option>`).join("")}
                    </select>
                </div>
            `;
            $filterContainer.append(dropdown);
        };


        // Add event listeners for filters
        $(".filter-btn").on("click", function () {
            $(".filter-btn").removeClass("activeCategory"); // Remove active class
            $(this).addClass("activeCategory"); // Add active class to clicked button
            const selectedCategory = $(this).data("category");
            filterByCategory(selectedCategory); // Filter events
        });

        $(".more-options").on("change", function () {
            const selectedCategory = $(this).val();
            $(".filter-btn").removeClass("activeCategory"); // Remove active class
            $(`.filter-btn[data-category='${selectedCategory}']`).addClass("activeCategory"); // Add active class
            filterByCategory(selectedCategory); // Filter events
        });
    }

    let events = []; // Will hold all event data
    let filteredEvents = []; // Will hold the filtered event list
    let isAscending = true; // For toggling sort order


    // Function to display events
    const displayEvents = (eventsToDisplay) => {
        $eventContainer.empty(); // Clear existing events
        eventsToDisplay.slice(0, 24).forEach((event) => {
            const eventHTML = `
                <a class="event-card" href="/Users/EventDetails/${event.id}" target="_blank">
                    <div class="event-image-container">
                        <img src="${event.images[0]?.url || ''}" alt="${event.images[0]?.alt || 'Event Image'}" class="event-image">
                    </div>
                    <div class="event-info">
                        <div class="event-date">
                            <span class="month dark-mode-clr">${event.date.month.slice(0, 3)}</span>
                            <span class="day">${event.date.day}</span>
                            <span class="year dark-mode-clr">${event.date.year}</span>
                        </div>
                        <div class="event-details">
                            <h3 class="event-title dark-mode-clr">${event.name}</h3>
                            <p class="event-description dark-mode-clr"> Organised by ${event.organizer.name}</p>
                        </div>
                    </div>
                </a>
            `;
            $eventContainer.append(eventHTML);
        });

        // Apply saved font sizes on page load
        applySavedFontSizes();

        // Reapply theme after creating buttons
        const currentTheme = localStorage.getItem("theme") || "light";
        applyTheme(currentTheme);

    };

    // Function to filter events by category
    const filterByCategory = (category) => {
        if (category === "all") {
            filteredEvents = events;
        } else {
            filteredEvents = events.filter((event) => event.type === category);
        }
        displayEvents(filteredEvents);
    };

    // Function to sort events by date
    const sortByDate = () => {
        filteredEvents.sort((a, b) => {
            const dateA = new Date(`${a.date.month} ${a.date.day}, ${a.date.year}`);
            const dateB = new Date(`${b.date.month} ${b.date.day}, ${b.date.year}`);
            return isAscending ? dateA - dateB : dateB - dateA;
        });
        isAscending = !isAscending; // Toggle sort order
        displayEvents(filteredEvents);
    };

    // Event listener for sort button
    $sortButton.on("click", function () {
        // Remove activeCategory class from all buttons
        $(".filter-btn, .sortContainer").removeClass("activeCategory");

        // Add activeCategory class to the clicked button
        $(this).addClass("activeCategory");

        // Call the sortByDate function
        sortByDate();
    });


    // display event search results
    let typingTimer; // Timer identifier
    const typingDelay = 300; // 500ms delay after user stops typing

    // Function to show the preloader
    const showPreloader = () => {
        $preloader.show();
        $searchResultsContainer.hide();
        $eventContainer.hide();
    };

    // Function to hide the preloader
    const hidePreloader = () => {
        $preloader.hide();
        $searchResultsContainer.show();
    };

    // Function to reset to initial state (no search results)
    const resetToInitialState = () => {
        $searchResultsContainer.empty();
        $searchResultsContainer.hide();
        $eventContainer.show();
    };

    // Function to display search results
    const displaySearchResults = (results, query) => {
        $searchResultsContainer.empty(); // Clear previous results

        if (results.length === 0) {
            // Display "No results" message
            $searchResultsContainer.append(`
                <div class="no-results">
                    Found 0 results for "${query}"
                </div>
            `);
        } else {
            // Display matched results
            results.forEach((event) => {
                const eventHTML = `
                <a class="event-card dark-mode-clr" href="/Users/EventDetails/${event.id}" target="_blank">
                    <img src="${event.images[0]?.url || ''}" alt="${event.images[0]?.alt || 'Event Image'}" class="event-image">
                    <div class="event-info">
                        <div class="event-date">
                            <span class="month dark-mode-clr">${event.date.month.slice(0, 3)}</span>
                            <span class="day">${event.date.day}</span>
                            <span class="year dark-mode-clr">${event.date.year}</span>
                        </div>
                        <div class="event-details">
                            <h3 class="event-title dark-mode-clr">${event.name}</h3>
                            <p class="event-description dark-mode-clr"> Organised by ${event.organizer.name}</p>
                        </div>
                    </div>
                </a>
                `;
                $searchResultsContainer.append(eventHTML);
            });
        }
    };

    // Function to perform search
    const searchEvents = () => {
        showPreloader(); // Show preloader while searching

        // Get search inputs
        const titleQuery = $searchTitle.val().trim().toLowerCase();
        const placeQuery = $searchPlace.val().trim().toLowerCase();

        // Filter events by title and place
        const filteredResults = events.filter((event) => {
            const matchesTitle = event.name.toLowerCase().includes(titleQuery);
            const matchesPlace = event.venue?.address?.toLowerCase().includes(placeQuery) || event.venue?.name?.toLowerCase().includes(placeQuery);
            return matchesTitle || matchesPlace;
        });

        // Simulate a delay to mimic loading
        setTimeout(() => {
            hidePreloader(); // Hide preloader
            if (titleQuery || placeQuery) {
                $eventContainer.hide(); // Hide initial events when searching
                displaySearchResults(filteredResults, `${titleQuery} ${placeQuery}`.trim()); // Display results
            } else {
                resetToInitialState(); // Reset to initial state if search is cleared
            }
        }, typingDelay);
    };

    // Event listener for search inputs
    $searchTitle.add($searchPlace).on("input", function () {
        clearTimeout(typingTimer); // Clear the previous timer
        typingTimer = setTimeout(searchEvents, typingDelay); // Start a new timer
    });


    // retrieving the id results for the event details page
    // Fetch event data and display event details
    const fetchAndDisplayEventDetails = async () => {
        try {
            
            // reapply theme
            const currentTheme = localStorage.getItem("theme") || "light";
            applyTheme(currentTheme);

        } catch (error) {
            console.error("Error fetching event data:", error);
        }
    };



    // Fetching data from data.json
    const fetchEvents = async () => {
        try {
            const response = await fetch("/Users/Events?handler=EventsJson"); // Fetch the data
            const data = await response.json(); // Parse JSON
            console.log("here is the data", data.events);

            events = data.events; // Store events globally
            filteredEvents = events; // Initialize filtered events

            createFilterButtons(events); // Create filters
            filterByCategory("all"); // Display all events initially

            // Apply saved font sizes on dynamically created elements
            $(targetClass).each(function () {
                const currentFontSize = parseFloat($(this).css("font-size")); // Get current font size in px
                $(this).attr("data-default-font-size", currentFontSize); // Store the default font size as a data attribute
            });

            // Apply saved font sizes on page load
            applySavedFontSizes();

            // Reapply theme after creating buttons
            const currentTheme = localStorage.getItem("theme") || "light";
            applyTheme(currentTheme);
        } catch (error) {
            console.error("Error fetching events:", error); // Log error for debugging
        }
    };



    fetchEvents();
    fetchAndDisplayEventDetails();

    // Hide preloader on initial load
    $preloader.hide();
    $searchResultsContainer.hide();

});