function changeValue(delta) {
    const input = document.getElementById('number');
    let value = parseInt(input.value) || 0;
    value += delta;
    if (value < input.min) value = input.min;
    if (value > input.max) value = input.max;
    input.value = value;
}

function showPopup() {
    document.getElementById('popup').style.display = 'block';
    document.getElementById('popupOverlay').style.display = 'block';
}

// Hàm để đóng popup
function closePopup() {
    document.getElementById('popup').style.display = 'none';
    document.getElementById('popupOverlay').style.display = 'none';
}