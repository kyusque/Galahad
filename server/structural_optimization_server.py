from galahad_translator import GldMoleculeTranslator
from aiohttp import web

from rdkit.Chem import AllChem

routes = web.RouteTableDef()
translator = GldMoleculeTranslator()
current_json = ''

@routes.get("/")
@routes.post("/")
async def parse_file(request: web.Request):
    global current_json
    if request.method == "POST":
        print("POST")
        data = await request.post()
        print(data["json"])
        try:
            gld_mol = translator.loads(data["json"])
        except Exception as e:
            print(e)
            return web.Response(text="Invalid Data")
        current_json = data["json"]
        return web.Response(text=f"{gld_mol.title} is registered")
    return web.Response(text=current_json)

@routes.get("/move")
async def parse_file(request: web.Request):
    global current_json
    print(current_json)
    gld_mol = translator.loads(current_json)
    AllChem.EmbedMolecule(gld_mol.mol, maxAttempts=5)
    current_json = translator.dumps(gld_mol)
    return web.Response(text=current_json)

app = web.Application()
app.add_routes(routes)
web.run_app(app)
