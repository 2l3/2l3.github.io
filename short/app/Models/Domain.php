<?php

namespace App\Models;

use App\Transformers\DomainTransformer;
use Eloquent;
use EloquentFilter\Filterable;
use Flugg\Responder\Contracts\Transformable;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Carbon;

/**
 * App\Models\Domain
 *
 * @property int $id
 * @property string $domain
 * @property string|null $redirect
 * @property int $status
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @method static Builder|Domain filter($input = [], $filter = null)
 * @method static Builder|Domain newModelQuery()
 * @method static Builder|Domain newQuery()
 * @method static Builder|Domain paginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Domain query()
 * @method static Builder|Domain simplePaginateFilter($perPage = null, $columns = [], $pageName = 'page', $page = null)
 * @method static Builder|Domain whereBeginsWith($column, $value, $boolean = 'and')
 * @method static Builder|Domain whereCreatedAt($value)
 * @method static Builder|Domain whereDomain($value)
 * @method static Builder|Domain whereEndsWith($column, $value, $boolean = 'and')
 * @method static Builder|Domain whereId($value)
 * @method static Builder|Domain whereLike($column, $value, $boolean = 'and')
 * @method static Builder|Domain whereRedirect($value)
 * @method static Builder|Domain whereStatus($value)
 * @method static Builder|Domain whereUpdatedAt($value)
 * @mixin Eloquent
 */
class Domain extends Model implements Transformable
{
    use Filterable;

    /**
     * @inheritDoc
     */
    public function transformer()
    {
        return DomainTransformer::class;
    }
}
