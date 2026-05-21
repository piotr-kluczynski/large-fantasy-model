
const viewport = document.getElementById('viewport');
const mapContainer = document.getElementById('map-container');
let isMapDragging = false;
let mapStartX, mapStartY;
let mapX = -2000;
let mapY = -2000;
window.mapScale = 1; 

function updateMapTransform() {
    mapContainer.style.transform = `translate(${mapX}px, ${mapY}px) scale(${window.mapScale})`;
}
updateMapTransform();

viewport.addEventListener('mousedown', (e) => {
    if (window.isPlacingMonster) {
        e.preventDefault(); e.stopImmediatePropagation();
        const mapRect = mapContainer.getBoundingClientRect();
        let clickX = (e.clientX - mapRect.left) / window.mapScale;
        let clickY = (e.clientY - mapRect.top) / window.mapScale;
        let snappedX = Math.round(clickX / 50) * 50;
        let snappedY = Math.round(clickY / 50) * 50;
        window.connection.invoke("SpawnToken", gameId, window.pendingMonsterData.name, window.pendingMonsterData.hp, window.pendingMonsterData.color, snappedX, snappedY).catch(console.error);
        window.isPlacingMonster = false; window.pendingMonsterData = null; viewport.style.cursor = "grab";
        return;
    }

    if (e.target.closest('.game-token') || e.target.closest('#right-sidebar') || e.target.closest('#combat-panel') || e.target.closest('#players-container') || e.target.closest('#exit-vtt') || e.target.closest('#toggle-sidebar-btn') || e.target.closest('#toggle-combat-btn')) return;
    if (e.shiftKey) return;
    isMapDragging = true; mapStartX = e.clientX - mapX; mapStartY = e.clientY - mapY;
});

window.addEventListener('mousemove', (e) => {
    if (!isMapDragging) return;
    mapX = e.clientX - mapStartX; mapY = e.clientY - mapStartY; updateMapTransform();
});

window.addEventListener('mouseup', () => { isMapDragging = false; });

viewport.addEventListener('wheel', (e) => {
    e.preventDefault();
    const zoomDirection = e.deltaY < 0 ? 1 : -1;
    const oldScale = window.mapScale;
    window.mapScale += zoomDirection * 0.1;
    window.mapScale = Math.min(Math.max(0.2, window.mapScale), 3.0);
    const mouseMapX = (e.clientX - mapX) / oldScale;
    const mouseMapY = (e.clientY - mapY) / oldScale;
    mapX = e.clientX - (mouseMapX * window.mapScale);
    mapY = e.clientY - (mouseMapY * window.mapScale);
    updateMapTransform();
}, { passive: false });

window.setMapBackground = function (url) {
    let newWidth = 5000; let newHeight = 5000;
    const applyBackgroundAndRescueTokens = () => {
        mapContainer.style.width = newWidth + 'px'; mapContainer.style.height = newHeight + 'px';
        mapX = (window.innerWidth - (newWidth * window.mapScale)) / 2; mapY = (window.innerHeight - (newHeight * window.mapScale)) / 2;
        updateMapTransform();
        if (url) {
            mapContainer.style.backgroundImage = `linear-gradient(rgba(255, 255, 255, 0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255, 255, 255, 0.1) 1px, transparent 1px), url('${url}')`;
            mapContainer.style.backgroundSize = '50px 50px, 50px 50px, 100% 100%'; mapContainer.style.backgroundRepeat = 'repeat, repeat, no-repeat'; mapContainer.style.backgroundColor = '#111';
        } else {
            mapContainer.style.backgroundImage = `linear-gradient(rgba(255, 255, 255, 0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255, 255, 255, 0.1) 1px, transparent 1px)`;
            mapContainer.style.backgroundSize = '50px 50px'; mapContainer.style.backgroundColor = '#2b2b2b';
        }
        document.querySelectorAll('.game-token').forEach(token => {
            let currentX = parseFloat(token.style.left); let currentY = parseFloat(token.style.top); let needsRescue = false;
            if (currentX >= newWidth - 100) { currentX = Math.max(0, newWidth - 100); needsRescue = true; }
            if (currentY >= newHeight - 100) { currentY = Math.max(0, newHeight - 100); needsRescue = true; }
            if (needsRescue) {
                currentX = Math.round(currentX / 50) * 50; currentY = Math.round(currentY / 50) * 50;
                token.style.transition = "left 0.4s ease-out, top 0.4s ease-out";
                token.style.left = currentX + 'px'; token.style.top = currentY + 'px';
                if (isDM && window.connection) { window.connection.invoke("UpdateTokenPosition", gameId, parseInt(token.getAttribute('data-token-id')), currentX, currentY).catch(console.error); }
                setTimeout(() => { token.style.transition = "transform 0.1s, box-shadow 0.1s"; }, 410);
            }
        });
    };
    if (url) { const img = new Image(); img.onload = function () { newWidth = img.width; newHeight = img.height; applyBackgroundAndRescueTokens(); }; img.src = url; }
    else { applyBackgroundAndRescueTokens(); }
};

if (typeof initialMapUrl !== 'undefined') window.setMapBackground(initialMapUrl);

document.getElementById('btn-change-map')?.addEventListener('click', () => { new bootstrap.Modal(document.getElementById('mapGalleryModal')).show(); });
document.querySelectorAll('.map-option').forEach(option => {
    option.addEventListener('click', function () {
        window.connection.invoke("ChangeMap", gameId, this.getAttribute('data-url')).catch(console.error);
        const modalInstance = bootstrap.Modal.getInstance(document.getElementById('mapGalleryModal'));
        if (modalInstance) modalInstance.hide();
    });
});

const rulerLayer = document.createElementNS("http://www.w3.org/2000/svg", "svg");
rulerLayer.style.position = "absolute"; rulerLayer.style.top = "0"; rulerLayer.style.left = "0"; rulerLayer.style.width = "100%"; rulerLayer.style.height = "100%"; rulerLayer.style.pointerEvents = "none"; rulerLayer.style.zIndex = "150"; rulerLayer.style.display = "none";
const rulerLine = document.createElementNS("http://www.w3.org/2000/svg", "line"); rulerLine.setAttribute("stroke", "#ffc107"); rulerLine.setAttribute("stroke-width", "4"); rulerLine.setAttribute("stroke-dasharray", "8, 6");
const rulerTextBg = document.createElementNS("http://www.w3.org/2000/svg", "rect"); rulerTextBg.setAttribute("fill", "#1a1a1a"); rulerTextBg.setAttribute("stroke", "#ffc107"); rulerTextBg.setAttribute("stroke-width", "1"); rulerTextBg.setAttribute("rx", "6");
const rulerText = document.createElementNS("http://www.w3.org/2000/svg", "text"); rulerText.setAttribute("fill", "white"); rulerText.setAttribute("font-size", "14"); rulerText.setAttribute("font-weight", "bold"); rulerText.setAttribute("font-family", "sans-serif"); rulerText.setAttribute("text-anchor", "middle"); rulerText.setAttribute("dominant-baseline", "central");
rulerLayer.appendChild(rulerLine); rulerLayer.appendChild(rulerTextBg); rulerLayer.appendChild(rulerText); mapContainer.appendChild(rulerLayer);

let isMeasuring = false; let measureStartX = 0; let measureStartY = 0;
viewport.addEventListener('mousedown', (e) => {
    if (e.shiftKey) {
        isMeasuring = true; const mapRect = mapContainer.getBoundingClientRect();
        measureStartX = (e.clientX - mapRect.left) / window.mapScale; measureStartY = (e.clientY - mapRect.top) / window.mapScale;
        rulerLine.setAttribute("x1", measureStartX); rulerLine.setAttribute("y1", measureStartY); rulerLine.setAttribute("x2", measureStartX); rulerLine.setAttribute("y2", measureStartY);
        rulerText.textContent = "0 ft"; rulerLayer.style.display = "block";
    }
});
window.addEventListener('mousemove', (e) => {
    if (!isMeasuring) return;
    const mapRect = mapContainer.getBoundingClientRect(); let currentX = (e.clientX - mapRect.left) / window.mapScale; let currentY = (e.clientY - mapRect.top) / window.mapScale;
    rulerLine.setAttribute("x2", currentX); rulerLine.setAttribute("y2", currentY);
    let dx = currentX - measureStartX; let dy = currentY - measureStartY;
    let feet = Math.round((Math.sqrt(dx * dx + dy * dy) / 50) * 5);
    let midX = measureStartX + dx / 2; let midY = measureStartY + dy / 2;
    rulerText.setAttribute("x", midX); rulerText.setAttribute("y", midY); rulerText.textContent = `${feet} ft`;
    let textWidth = feet.toString().length * 8 + 30;
    rulerTextBg.setAttribute("x", midX - textWidth / 2); rulerTextBg.setAttribute("y", midY - 12); rulerTextBg.setAttribute("width", textWidth); rulerTextBg.setAttribute("height", "24");
});
window.addEventListener('mouseup', () => { if (isMeasuring) { isMeasuring = false; rulerLayer.style.display = "none"; } });

// --- LOGIKA PANELI BOCZNYCH ---
const sidebar = document.getElementById('right-sidebar');
const toggleBtn = document.getElementById('toggle-sidebar-btn');
const toggleIcon = toggleBtn?.querySelector('i');

const combatPanel = document.getElementById('combat-panel');
const toggleCombatBtn = document.getElementById('toggle-combat-btn');
const closeCombatBtn = document.getElementById('close-combat-btn');
const toggleCombatIcon = toggleCombatBtn?.querySelector('i');

let isChatOpen = false;
let isCombatOpen = false;

function updatePanels() {
    if (isChatOpen) { sidebar.style.right = '0'; } else { sidebar.style.right = '-400px'; }
    if (isCombatOpen) { combatPanel.style.left = '0'; } else { combatPanel.style.left = '-350px'; }
}

toggleBtn?.addEventListener('click', () => {
    isChatOpen = !isChatOpen;
    if (isChatOpen) toggleIcon.classList.replace('bi-chat-left-dots', 'bi-chevron-right');
    else toggleIcon.classList.replace('bi-chevron-right', 'bi-chat-left-dots');
    updatePanels();
});

toggleCombatBtn?.addEventListener('click', () => {
    isCombatOpen = !isCombatOpen;
    if (isCombatOpen) { toggleCombatIcon.classList.replace('bi-swords', 'bi-chevron-left'); }
    else { toggleCombatIcon.classList.replace('bi-chevron-left', 'bi-swords'); }
    updatePanels();
});

closeCombatBtn?.addEventListener('click', () => {
    isCombatOpen = false;
    toggleCombatIcon.classList.replace('bi-chevron-left', 'bi-swords');
    updatePanels();
});

window.switchTab = function (tabName) {
    document.querySelectorAll('.sidebar-tabs button').forEach(b => b.classList.remove('active'));
    document.querySelectorAll('.sidebar-content').forEach(c => c.classList.remove('active'));
    document.getElementById('tab-' + tabName).classList.add('active');
    document.getElementById('content-' + tabName).classList.add('active');
};