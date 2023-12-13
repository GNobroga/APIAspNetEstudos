# Pagination

Páginação permite perfomatizar a API, pois pode ter muitos dados na fonte de dados e isso pode gerar travamentos ou demoras, a paginação evita isso.

Criando de forma manual.

```cs
    public ActionResult<IEnumerable<Model>> Action([FromQuery] page, [FromQuery] size)
    {
        return Enumerable.Range(0, 100).Skip(size * (page - 1)).Take(size);
    }
```



