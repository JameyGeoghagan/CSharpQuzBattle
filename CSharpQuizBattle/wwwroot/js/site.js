window.onload = function () {
    const starCount = 100;
    const background = document.getElementById('background-animation');

    for (let i = 0; i < starCount; i++) {
        const star = document.createElement('div');
        star.className = 'stars';
        star.style.left = Math.random() * window.innerWidth + 'px';
        star.style.top = Math.random() * window.innerHeight + 'px';
        star.style.animationDuration = (Math.random() * 5 + 2) + 's';
        background.appendChild(star);
    }
};
