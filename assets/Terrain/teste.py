from PIL import Image

INPUT_FILE = "Terrain (16x16).png"
OUTPUT_FILE = "tiles.txt"
TILE_SIZE = 16

def is_empty(tile):
    for p in tile.getdata():
        if len(p) == 4 and p[3] != 0:
            return False
    return True

def main():
    img = Image.open(INPUT_FILE).convert("RGBA")

    width, height = img.size

    lines = []

    for y in range(0, height, TILE_SIZE):
        for x in range(0, width, TILE_SIZE):

            tile = img.crop((x, y, x + TILE_SIZE, y + TILE_SIZE))

            if is_empty(tile):
                continue

            lines.append(f"{x},{y},{x + TILE_SIZE},{y + TILE_SIZE}")

    with open(OUTPUT_FILE, "w") as f:
        f.write("\n".join(lines))

    print(f"Saved {len(lines)} tiles to {OUTPUT_FILE}")

if __name__ == "__main__":
    main()