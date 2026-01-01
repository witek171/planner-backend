KEY_DIR="./Schedule.API/Data"
PRIVATE_KEY_FILE="$KEY_DIR/private.key"
PUBLIC_KEY_FILE="$KEY_DIR/public.key"

mkdir -p "$KEY_DIR"

if [ -f "$PRIVATE_KEY_FILE" ] && [ -f "$PUBLIC_KEY_FILE" ]; then
    echo "RSA keys already exist. Skipping generation."
    exit 0
fi

echo "Generating RSA keys..."

openssl genpkey -algorithm RSA -out "$PRIVATE_KEY_FILE" -pkcs8 -pass pass: 2048

openssl rsa -in "$PRIVATE_KEY_FILE" -pubout -out "$PUBLIC_KEY_FILE"

echo "RSA keys have been generated:"
echo "  - Private key: $PRIVATE_KEY_FILE"
echo "  - Public key: $PUBLIC_KEY_FILE"

chmod 600 "$PRIVATE_KEY_FILE"
chmod 644 "$PUBLIC_KEY_FILE"