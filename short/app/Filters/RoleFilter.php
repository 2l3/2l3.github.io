<?php

namespace App\Filters;

use EloquentFilter\ModelFilter;
use Illuminate\Database\Eloquent\Builder;

class RoleFilter extends ModelFilter
{
    /**
     * Related Models that have ModelFilters as well as the method on the ModelFilter
     * As [relationMethod => [input_key1, input_key2]].
     *
     * @var array
     */
    public $relations = [];

    /**
     * We filter by name.
     *
     * @param $search
     * @return RoleFilter
     */
    public function search($search)
    {
        return $this->where(function ($query) use ($search) {
            /** @var Builder $query */
            return $query->where('name', 'LIKE', '%'.$search.'%');
        });
    }
}
