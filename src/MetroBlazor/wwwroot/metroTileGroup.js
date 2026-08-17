// While any tile in the group is in edit mode, a click or contextmenu outside a
// tile (the grid's gaps and the rest of the page included) exits edit mode.
// Returns a disposable handle. The .editing check keeps .NET round-trips to
// editing sessions only.
export function registerDismiss(grid, dotNetRef) {
    const handler = (e) => {
        if (!grid.querySelector('.metro-tile-group-item.editing')) {
            return;
        }

        const item = e.target.closest && e.target.closest('.metro-tile-group-item');
        if (item && grid.contains(item)) {
            return;
        }

        dotNetRef.invokeMethodAsync('ExitEditModeFromJs').catch(() => { });
    };
    document.addEventListener('click', handler, true);
    document.addEventListener('contextmenu', handler, true);
    return {
        dispose: () => {
            document.removeEventListener('click', handler, true);
            document.removeEventListener('contextmenu', handler, true);
        }
    };
}

// Measures the actual cell pitch (base track + gap) and viewport origin of a
// rows-mode tile group grid. Drag snapping needs both: hosts can shrink the
// track via --metro-tile-track, and touch pointer events under implicit capture
// report offsets relative to the capture target, so ClientX/Y minus the grid
// origin is the only reliable cell math for touch drags.
export function measureGrid(grid) {
    if (!grid) {
        return { pitch: 68, left: 0, top: 0 };
    }

    const style = getComputedStyle(grid);
    const firstRow = (style.gridTemplateRows || '').split(' ')[0];
    const track = parseFloat(firstRow) || 60;
    const gap = parseFloat(style.columnGap) || 0;
    const rect = grid.getBoundingClientRect();
    return { pitch: track + gap, left: rect.left, top: rect.top };
}
