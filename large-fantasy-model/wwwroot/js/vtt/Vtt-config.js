const vttConfigEl = document.body;
const gameId = parseInt(vttConfigEl.getAttribute('data-game-id'));
const currentUserId = vttConfigEl.getAttribute('data-current-user-id');
const myUsername = vttConfigEl.getAttribute('data-my-username');
const isDM = vttConfigEl.getAttribute('data-is-dm') === 'true';
const initialMapUrl = vttConfigEl.getAttribute('data-initial-map-url');
