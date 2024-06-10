document.addEventListener('DOMContentLoaded', function () {
    const menuButton = document.getElementById('menuButton');
    const sideMenu = document.getElementById('sideMenu');
    const closeButton = document.querySelector('.close-btn');
    const tabsContainer = document.getElementById('tabsContainer');
    const content = document.getElementById('content');

    closeButton.onclick = function () {
        sideMenu.style.width = '0';
    };

    menuButton.onclick = function () {
        sideMenu.style.width = '250px';
    };

    function updateTime() {
        const now = new Date();
        const timeString = now.toLocaleTimeString('en-GB', { timeZone: 'Europe/Warsaw' });
        const dateString = now.toLocaleDateString('en-GB', { timeZone: 'Europe/Warsaw' });

        document.getElementById('time').textContent = timeString;
        document.getElementById('date').textContent = dateString;
    }

    setInterval(updateTime, 1000);
    updateTime();

    function addTab(name, url) {
        // Check if tab already exists
        if (document.getElementById(name + 'Tab')) {
            // If tab exists, switch to it
            switchTab(name, url);
            return;
        }

        // Create tab element
        const tab = document.createElement('div');
        tab.className = 'tab';
        tab.id = name + 'Tab';
        tab.innerHTML = name + ' <span class="close">&times;</span>';

        // Add click event to switch to tab
        tab.onclick = function () {
            switchTab(name, url);
        };

        // Add close event to remove tab
        tab.querySelector('.close').onclick = function (e) {
            e.stopPropagation();
            removeTab(name);
        };

        tabsContainer.appendChild(tab);

        // Switch to new tab
        switchTab(name, url);
    }

    function switchTab(name, url) {
        // Set active class for tabs
        Array.from(tabsContainer.children).forEach(child => {
            child.classList.remove('active');
        });
        document.getElementById(name + 'Tab').classList.add('active');

        // Load content for the tab
        content.innerHTML = `<iframe src="${url}" style="width: 100%; height: 100%; border: none;"></iframe>`;
    }

    function removeTab(name) {
        const tab = document.getElementById(name + 'Tab');
        if (tab) {
            tab.remove();
        }

        // If removed tab was active, switch to first tab
        if (!tabsContainer.querySelector('.tab.active')) {
            const firstTab = tabsContainer.querySelector('.tab');
            if (firstTab) {
                firstTab.click();
            } else {
                // If no tabs left, clear content
                content.innerHTML = '';
            }
        }
    }

    document.getElementById('homeButton').onclick = function () {
        addTab('Home', '/Home/Index');
    };

    document.getElementById('privateButton').onclick = function () {
        addTab('Private', '/Home/Privacy');
    };
});
